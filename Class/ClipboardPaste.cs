using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Clipboards.Class
{
    internal class ClipboardPaste
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 CloseClipboard();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Int32 GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern Int32 GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern UIntPtr GlobalSize(IntPtr hMem);

        const uint CF_TEXT = 1;

        /// <summary>
        /// 实现控制台自动粘贴板功能
        /// </summary>
        /// <returns></returns>
        public static string PasteTextFromClipboard()
        {
            string result = "";
            if (IsClipboardFormatAvailable(CF_TEXT) == 0)
            {
                return result;
            }
            if (OpenClipboard((IntPtr)0) == 0)
            {
                return result;
            }

            IntPtr hglb = GetClipboardData(CF_TEXT);
            if (hglb != (IntPtr)0)
            {
                UIntPtr size = GlobalSize(hglb);
                IntPtr s = (IntPtr)GlobalLock(hglb);
                byte[] buffer = new byte[(int)size];
                Marshal.Copy(s, buffer, 0, (int)size);
                if (s != null)
                {
                    result = ASCIIEncoding.ASCII.GetString(buffer);
                    GlobalUnlock(hglb);
                }
            }

            CloseClipboard();
            return result;
        }
    }
}
