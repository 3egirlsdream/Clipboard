using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Clipboards.Class
{
    public class HotKeySettingsManager
    {
        private static HotKeySettingsManager m_Instance;
        /// <summary>
        /// 单例实例
        /// </summary>
        public static HotKeySettingsManager Instance
        {
            get { return m_Instance ?? (m_Instance = new HotKeySettingsManager()); }
        }

        /// <summary>
        /// 加载默认快捷键
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<HotKeyModel> LoadDefaultHotKey()
        {
            var hotKeyList = new ObservableCollection<HotKeyModel>();
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.全屏.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Q });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.截图.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Z });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.播放.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Space });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.前进.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.D });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.后退.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.A });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.保存.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.B });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.打开.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.X });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.新建.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.H });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.删除.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.G });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.复制.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.C });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.剪贴板.ToString(), IsUsable = true, IsSelectCtrl = false, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.V });
            return hotKeyList;
        }

        /// <summary>
        /// 通知注册系统快捷键委托
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        public delegate bool RegisterGlobalHotKeyHandler(ObservableCollection<HotKeyModel> hotKeyModelList);
        public event RegisterGlobalHotKeyHandler RegisterGlobalHotKeyEvent;
        public bool RegisterGlobalHotKey(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            if (RegisterGlobalHotKeyEvent != null)
            {
                return RegisterGlobalHotKeyEvent(hotKeyModelList);
            }
            return false;
        }

    }

    public enum EHotKeySetting
    {
        //全屏 = 0,
        //截图 = 1,
        //播放 = 2,
        //前进 = 3,
        //后退 = 4,
        //保存 = 5,
        //打开 = 6,
        //新建 = 7,
        //删除 = 8,
        复制 = 1,
        剪贴板 = 0
    }
}
