using System;
using System.Collections.Generic;
using System.Text;

namespace Clipboards.Class
{
    /// 
    /// 快捷键模型
    /// 
    public class HotKeyModel
    {
        /// 
        /// 设置项名称
        /// 
        public string Name { get; set; }

        /// 
        /// 设置项快捷键是否可用
        /// 
        public bool IsUsable { get; set; }

        /// 
        /// 是否勾选Ctrl按键
        /// 
        public bool IsSelectCtrl { get; set; }


        public bool IsSelectWin { get; set; }


        /// 
        /// 是否勾选Shift按键
        /// 
        public bool IsSelectShift { get; set; }

        /// 
        /// 是否勾选Alt按键
        /// 
        public bool IsSelectAlt { get; set; }

        /// 
        /// 选中的按键
        /// 
        public EKey SelectKey { get; set; }

        /// 
        /// 快捷键按键集合
        /// 
        public static Array Keys
        {
            get
            {
                return Enum.GetValues(typeof(EKey));
            }
        }
    }

    ///  
    /// 自定义按键枚举
    /// 
    public enum EKey
    {
        Space = 32,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
    }
}
