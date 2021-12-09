using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Clipboards.Class
{
    public class NotifyIconViewModel : ValidationBase
    {
        /// <summary>
        /// 如果窗口没显示，就显示窗口
        /// </summary>
        public SimpleCommand ShowWindowCommand
        {
            get
            {
                return new SimpleCommand
                {
                    CanExecuteDelegate = x => Application.Current.MainWindow != null && Application.Current.MainWindow.Visibility != Visibility.Visible,
                    ExecuteDelegate = o =>
                    {
                        Application.Current.MainWindow.Visibility = Visibility.Visible;
                    }
                };
            }
        }

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public SimpleCommand HideWindowCommand
        {
            get
            {
                return new SimpleCommand
                {
                    ExecuteDelegate = x => Application.Current.MainWindow.Visibility = Visibility.Collapsed,
                    CanExecuteDelegate = o => Application.Current.MainWindow != null
                };
            }
        }


        public SimpleCommand OpenSettingCommand
        {
            get
            {
                return new SimpleCommand
                {
                    ExecuteDelegate = x =>
                    {
                        var setting = new Setting();
                        setting.ShowDialog();
                    },
                    CanExecuteDelegate = o => Application.Current.MainWindow != null,
                };
            }
        }


        private bool isChecked = AutoStart.IsExistKey(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName);
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                NotifyPropertyChanged(nameof(IsChecked));
                AutoStart.SetMeStart(value);
            }
        }


        /// <summary>
        /// 关闭软件
        /// </summary>
        public SimpleCommand ExitApplicationCommand
        {
            get
            {
                return new SimpleCommand { ExecuteDelegate = x => Application.Current.Shutdown() };
            }
        }
    }
}
