using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace StartPro.Api;

public static class PEIcon
{
    public static bool FromFile(string fileName, out BitmapSource source)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            source = null;
            return false;
        }
        Icon icon = GetIcon(fileName);
        return FromIcon(icon, out source);
    }

    public static bool FromIcon(Icon icon, out BitmapSource source)
    {
        source = null;
        IntPtr handle = icon.Handle;
        try
        {
            if (handle == IntPtr.Zero)
                return false;
            source = Imaging.CreateBitmapSourceFromHIcon(handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions( ));
            if (source is null)
            {
                return false;
            }
            else if (source.CanFreeze)
            {
                source.Freeze( );
                return true;
            }
        }
        catch { }
        finally
        {
            icon.Dispose( );
        }
        return false;
    }

    public static Icon GetIcon(string fileName)
    {
        return Icon.ExtractIcon(fileName, 0, 256);
    }
}
