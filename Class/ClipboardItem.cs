using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Clipboards.Class
{
    public class ClipboardItem
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public ClipboardType Type { get; set; }
        public string TextVisible { get => Type == ClipboardType.Text ? "Visible" : "Hidden"; }
        public string ImageVisible { get => Type == ClipboardType.Image ? "Visible" : "Hidden"; }
        //public ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        //{
        //    this.Image.s
        //    IntPtr hBitmap = bitmap.GetHbitmap();
        //    ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
        //        hBitmap,
        //        IntPtr.Zero,
        //        Int32Rect.Empty,
        //        BitmapSizeOptions.FromEmptyOptions());

        //    if (!DeleteObject(hBitmap))
        //    {
        //        throw new System.ComponentModel.Win32Exception();
        //    }
        //    return wpfBitmap;
        //}
        public int HashCode
        {
            get => (string.IsNullOrEmpty(Text) ? 0 : Text.GetHashCode()) + (Image == null ? 0 :Image.GetHashCode());
        }

    }

    public enum ClipboardType
    {
        Text = 0,
        Image = 1,
    }
}
