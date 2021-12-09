using Clipboards.Class;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clipboards
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainVM vm;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                vm = new MainVM(this);
                this.DataContext = vm;
                Top = System.Windows.SystemParameters.PrimaryScreenHeight - this.Height - 50;
                Left = SystemParameters.PrimaryScreenWidth - this.Width - 20;
                Loaded += MainWindow_Loaded;
                MouseLeave += MainWindow_MouseLeave;
            }
            catch (Exception ex)
            {
                LogHelper.Instance._logger.Error(ex.Message);
            }
        }

        private void MainWindow_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;
        }

        /// <summary>
        /// 通知注册系统快捷键事件处理函数
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return InitHotKey(hotKeyModelList);
        }

        #region 消息钩子预定义参数
        private const int WM_DRAWCLIPBOARD = 0x308;
        private const int WM_CHANGECBCHAIN = 0x30D;

        private IntPtr mNextClipBoardViewerHWnd;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern bool ChangeClipboardChain(IntPtr HWnd, IntPtr HWndNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static public extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        #endregion
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            //挂消息钩子
            mNextClipBoardViewerHWnd = SetClipboardViewer(source.Handle);
            source.AddHook(WndProc);

            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc2);
        }

        /// <summary>
        ///  参考：https://blog.csdn.net/xlm289348/article/details/8050957
        ///  MSG=0x308无法收到消息，原因是0x308是在剪贴板发生变化时将消息发送到监听列表中的第一个窗口
        ///  所有这里要收到0x308必须将窗口放到监听列表里
        ///  即在OnSourceInitialized(EventArgs e) 中调用SetClipboardView
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            
            var clip = new ClipboardItem();
            switch (msg)
            {
                case WM_DRAWCLIPBOARD:
                    {
                        SendMessage(mNextClipBoardViewerHWnd, msg, wParam.ToInt32(), lParam.ToInt32());
                        if (!HotKeyHelper.IsIgnorance)
                        {
                            try
                            {
                                //文本内容检测
                                if (System.Windows.Clipboard.ContainsText())
                                {
                                    clip.Type = ClipboardType.Text;
                                    clip.Text = System.Windows.Clipboard.GetText().Trim();
                                    //做进一步操作
                                }
                                else if (Clipboard.ContainsImage())
                                {
                                    clip.Type = ClipboardType.Image;
                                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                                    {
                                        var img = Clipboard.GetImage();
                                        BitmapEncoder encoder = new BmpBitmapEncoder();
                                        encoder.Frames.Add(BitmapFrame.Create(img));
                                        encoder.Save(ms);
                                        var map = new System.Drawing.Bitmap(ms);
                                        var path = System.Environment.CurrentDirectory + "\\cache\\" + img.GetHashCode() + ".png";
                                        map.Save(path);
                                        clip.Image = path;
                                    }
                                }
                                if (vm.ClipboardsItems.All(c => c.HashCode != clip.HashCode) && clip.HashCode != 0)
                                {
                                    vm.ClipboardsItems.Insert(0, clip);
                                }
                            }
                            catch(Exception ex)
                            {
                                LogHelper.Instance._logger.Error(ex);
                            }
                        }
                        else
                        {
                            HotKeyHelper.IsIgnorance = false;
                        }
                    }
                    break;
                case WM_CHANGECBCHAIN:
                    {
                        if (wParam == (IntPtr)mNextClipBoardViewerHWnd)
                        {
                            mNextClipBoardViewerHWnd = lParam;
                        }
                        else
                        {
                            SendMessage(mNextClipBoardViewerHWnd, msg, wParam.ToInt32(), lParam.ToInt32());
                        }
                    }
                    break;
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
                if (source != null)
                {
                    //移除消息钩子
                    ChangeClipboardChain(source.Handle, mNextClipBoardViewerHWnd);
                    source.RemoveHook(WndProc);
                }
                //清除缓存
                var path = System.Environment.CurrentDirectory + "\\cache";
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                var info = directoryInfo.GetFiles();
                foreach(var infoItem in info)
                {
                    File.Delete(infoItem.FullName);
                }
            }
            catch(Exception ex)
            {
                LogHelper.Instance._logger.Error(ex.Message);
            }
            base.OnClosed(e);
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                WindowState = WindowState.Normal;
                DragMove();
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        
        /// 
        /// 所有控件初始化完成后调用
        /// 
        /// 
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            // 注册热键
            InitHotKey();
        }


        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();
        /// 
        /// 初始化注册快捷键
        /// 
        /// 待注册热键的项
        /// true:保存快捷键的值；false:弹出设置窗体
        private bool InitHotKey(ObservableCollection<HotKeyModel> hotKeyModelList = null)
        {
            var list = hotKeyModelList ?? HotKeySettingsManager.Instance.LoadDefaultHotKey();
            // 注册全局快捷键
            string failList = HotKeyHelper.RegisterGlobalHotKey(list, m_Hwnd, out m_HotKeySettings);
            if (string.IsNullOrEmpty(failList))
                return true;

            MessageBoxResult mbResult = MessageBox.Show("提示", string.Format("无法注册下列快捷键\n\r{0}是否要改变这些快捷键？", failList), MessageBoxButton.YesNo);
            
            return true;
        }


        /// 
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// 
        /// 窗口句柄
        /// 消息
        /// 附加参数1
        /// 附加参数2
        /// 是否处理
        /// 返回句柄
        private IntPtr WndProc2(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            //var hotkeySetting = new EHotKeySetting();
            switch (msg)
            {
                case HotKeyManager.WM_HOTKEY:
                    int sid = wideParam.ToInt32();
                    //if (sid == m_HotKeySettings[EHotKeySetting.复制])
                    //{
                    //    hotkeySetting = EHotKeySetting.复制;
                    //    //TODO 执行全屏操作
                    //}
                    if (sid == m_HotKeySettings[EHotKeySetting.剪贴板])
                    {
                        //hotkeySetting = EHotKeySetting.剪贴板;
                        if(Application.Current.MainWindow == null)
                        {
                            Application.Current.MainWindow = new Window();
                        } 
                        Application.Current.MainWindow.Show();
                    }

                    //MessageBox.Show(string.Format("触发【{0}】快捷键", hotkeySetting.ToString()));
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

    }
}
