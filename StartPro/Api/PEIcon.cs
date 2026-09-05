using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.Shell;

using static Windows.Win32.PInvoke;

namespace StartPro.Api;

public static class PEIcon
{
    public static bool Complex(string path, out BitmapSource source)
    {
        if (string.IsNullOrEmpty(path))
        {
            source = new BitmapImage( );
            return false;
        }

        var imgPtr = ComplexBitmap(path);
        return FromBitmap(imgPtr, out source);
    }

    public static IntPtr ComplexBitmap(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return IntPtr.Zero;
        }

        IShellItemImageFactory? factory = null;
        try
        {
            if (SHCreateItemFromParsingName(path, null, out IShellItemImageFactory item).Failed)
            {
                return IntPtr.Zero;
            }

            factory = item;
            unsafe
            {
                HBITMAP hBitmap;
                factory.GetImage(new SIZE(256, 256), SIIGBF.SIIGBF_BIGGERSIZEOK, &hBitmap);
                return (IntPtr) hBitmap;
            }
        }
        catch { }
        finally
        {
            if (factory is not null)
            {
                Marshal.ReleaseComObject(factory);
            }
        }

        return IntPtr.Zero;
    }

    public static bool Direct(string path, out BitmapSource source)
    {
        if (string.IsNullOrEmpty(path))
        {
            source = new BitmapImage( );
            return false;
        }

        var icon = DirectIcon(path);
        return FromIcon(icon, out source);
    }

    public static Icon? DirectIcon(string path)
    {
        try
        {
            return path.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)
                ? Icon.ExtractIcon(path, 0, 256) : null;
        }
        catch
        {
            return null;
        }
    }

    public static bool FromBitmap(IntPtr bitmap, out BitmapSource source)
    {
        source = new BitmapImage( );
        if (bitmap == IntPtr.Zero)
        {
            return false;
        }

        try
        {
            source = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions( ));

            if (source.CanFreeze)
            {
                source.Freeze( );
            }

            return true;
        }
        catch { }
        finally
        {
            DeleteObject((HGDIOBJ) bitmap);
        }

        return false;
    }

    public static bool FromIcon(Icon? icon, out BitmapSource source)
    {
        source = new BitmapImage( );
        if (icon is null)
        {
            return false;
        }

        var handle = icon.Handle;
        if (handle == IntPtr.Zero)
        {
            return false;
        }

        try
        {
            source = Imaging.CreateBitmapSourceFromHIcon(
                handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions( ));

            if (source.CanFreeze)
            {
                source.Freeze( );
            }

            return true;
        }
        catch { }
        finally
        {
            icon.Dispose( );
        }

        return false;
    }
}
