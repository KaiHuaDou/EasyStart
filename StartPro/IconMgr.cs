using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static StartPro.NativeMethods;

namespace StartPro;

public static class IconMgr
{
    public static ImageSource Get(string path)
    {
        int iconCnt = PrivateExtractIcons(path, 0, 0, 0, null, null, 0, 0);
        IntPtr[] icons = new IntPtr[iconCnt];
        int okCnt = PrivateExtractIcons(path, 0, 256, 256, icons, null, iconCnt, 0);
        for (int i = 0; i < okCnt; i++)
        {
            if (icons[i] == IntPtr.Zero) continue;
            Icon icon = Icon.FromHandle(icons[i]);
            return Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle, new Int32Rect( ), BitmapSizeOptions.FromEmptyOptions( ));
        }
        return new BitmapImage( );
    }
}
