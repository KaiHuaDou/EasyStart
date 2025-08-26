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
    public static bool Complex(string path, out BitmapSource source)
    {
        if (string.IsNullOrEmpty(path))
        {
            source = null;
            return false;
        }
        IntPtr imgPtr = ComplexBitmap(path);
        return FromBitmap(imgPtr, out source);
    }

    public static IntPtr ComplexBitmap(string path)
    {
        if (string.IsNullOrEmpty(path))
            return IntPtr.Zero;

        IntPtr pImgFactory = IntPtr.Zero;
        try
        {
            Guid iid = IID_IShellItemImageFactory;
            SHCreateItemFromParsingName(path, IntPtr.Zero, ref iid, out pImgFactory);
            if (pImgFactory == IntPtr.Zero
                || Marshal.GetObjectForIUnknown(pImgFactory) is not IShellItemImageFactory factory)
            {
                return IntPtr.Zero;
            }

            Marshal.Release(pImgFactory);
            pImgFactory = IntPtr.Zero;

            SIZE size = new(256, 256);
            IntPtr hBitmap = IntPtr.Zero;
            factory.GetImage(size, SIIGBF.RESIZETOFIT | SIIGBF.BIGGERSIZEOK, out hBitmap);
            return hBitmap;

        }
        catch { }
        finally
        {
            if (pImgFactory != IntPtr.Zero)
                Marshal.Release(pImgFactory);
        }
        return IntPtr.Zero;
    }

    public static bool Direct(string path, out BitmapSource source)
    {
        if (string.IsNullOrEmpty(path))
        {
            source = null;
            return false;
        }
        Icon icon = DirectIcon(path);
        return FromIcon(icon, out source);
    }

    public static Icon DirectIcon(string path)
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
        source = null;
        if (bitmap == IntPtr.Zero)
            return false;
        try
        {
            source = Imaging.CreateBitmapSourceFromHBitmap(
                bitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions( ));

            if (source is null)
                return false;
            else if (source.CanFreeze)
                source.Freeze( );
            return true;
        }
        catch { }
        finally
        {
            DeleteObject(bitmap);
        }
        return false;
    }

    public static bool FromIcon(Icon icon, out BitmapSource source)
    {
        source = null;
        if (icon is null)
            return false;
        IntPtr handle = icon.Handle;
        if (handle == IntPtr.Zero)
            return false;
        try
        {
            source = Imaging.CreateBitmapSourceFromHIcon(
                handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions( ));

            if (source is null)
                return false;
            else if (source.CanFreeze)
                source.Freeze( );
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
