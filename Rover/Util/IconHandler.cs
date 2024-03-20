using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rover.Util
{
    internal class IconHandler
    {
        public static ImageSource GetIcon(string path, bool bSmall)
        {
            if ((System.IO.File.GetAttributes(path) & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
                return GetSystemIcon(Interop.SHSTOCKICONID.SIID_FOLDER, Interop.SHGSI_ICONSIZE.HGSI_SMALLICON);

            Interop.SHFILEINFO info = new Interop.SHFILEINFO(true);
            int cbFileInfo = Marshal.SizeOf(info);
            Interop.SHGFI flags;
            if (bSmall)
                flags = Interop.SHGFI.Icon | Interop.SHGFI.SmallIcon | Interop.SHGFI.UseFileAttributes;
            else
                flags = Interop.SHGFI.Icon | Interop.SHGFI.LargeIcon | Interop.SHGFI.UseFileAttributes;

            if (Interop.SHGetFileInfo(path, 256, out info, (uint)cbFileInfo, flags) == 0)
                return null;
            IntPtr iconHandle = info.hIcon;
            //if (IntPtr.Zero == iconHandle) // not needed, always return icon (blank)
            //  return DefaultImgSrc;
            ImageSource img = Imaging.CreateBitmapSourceFromHIcon(
                        iconHandle,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
            Interop.DestroyIcon(iconHandle);
            return img;
        }

        public static ImageSource GetSystemIcon(Interop.SHSTOCKICONID type, Interop.SHGSI_ICONSIZE size)
        {
            var info = new Interop.SHSTOCKICONINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            if (Interop.SHGetStockIconInfo(type, Interop.SHGSI_ICON | (uint)size, ref info) != 0)
                return null;
            IntPtr iconHandle = info.hIcon;

            ImageSource img = Imaging.CreateBitmapSourceFromHIcon(
                      iconHandle,
                      Int32Rect.Empty,
                      BitmapSizeOptions.FromEmptyOptions());
            Interop.DestroyIcon(iconHandle);
            return img;
        }
    }
}
