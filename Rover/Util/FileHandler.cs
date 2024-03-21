using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Rover.Util
{
    public class FileHandler
    {
        public static string GetTypeName(string path)
        {
            try
            {
                if ((System.IO.File.GetAttributes(path) & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
                    return "File Folder";

                Interop.SHFILEINFO info = new Interop.SHFILEINFO(true);
                int cbFileInfo = Marshal.SizeOf(info);
                Interop.SHGFI flags = Interop.SHGFI.TypeName;
                if (Interop.SHGetFileInfo(path, 256, out info, (uint)cbFileInfo, flags) == 0)
                    return null;
                IntPtr iconHandle = info.hIcon;
                Interop.DestroyIcon(iconHandle);
                return info.szTypeName;
            }
            catch (Exception e)
            {
                return "Unknown";
            }
        }
    }
}
