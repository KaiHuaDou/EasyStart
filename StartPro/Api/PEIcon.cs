using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static StartPro.External.NativeMethods;

namespace StartPro.Api;

public static class PEIcon
{
    public static BitmapSource Get(string path)
        => ToSource(GetIcon(path));

    public static BitmapSource ToSource(Icon icon)
    {
        try
        {
            if (icon == null)
                return new BitmapImage( );
            BitmapSource bitmap = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle, new Int32Rect( ), BitmapSizeOptions.FromEmptyOptions( ));
            DestroyIcon(icon.Handle);
            return bitmap;
        }
        catch (COMException) { }
        catch (ArgumentException) { }
        return new BitmapImage( );
    }

    public static Icon GetIcon(string path)
    {
        int iconCnt = PrivateExtractIcons(path, 0, 0, 0, null, null, 0, 0);
        IntPtr[] icons = new IntPtr[iconCnt];
        int okCnt = PrivateExtractIcons(path, 0, 256, 256, icons, null, iconCnt, 0);
        for (int i = 0; i < okCnt; i++)
        {
            if (icons[i] == IntPtr.Zero)
                continue;
            return Icon.FromHandle(icons[i]);
        }
        return null;
    }
}
