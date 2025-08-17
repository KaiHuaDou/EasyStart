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
        if (icon is null)
            return false;
        IntPtr handle = icon.Handle;
        if (handle == IntPtr.Zero)
            return false;
        try
        {
            source = Imaging.CreateBitmapSourceFromHIcon(handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions( ));
            if (source is null)
            {
                return false;
            }
            else if (source.CanFreeze)
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

    public static Icon GetIcon(string fileName)
    {
        try
        {
            return Icon.ExtractIcon(fileName, 0, 256);
        }
        catch
        {
            return null;
        }
    }

    public static bool GetThumbnail(string filePath, out BitmapSource source)
    {
        source = null;

        if (string.IsNullOrEmpty(filePath))
            return false;

        IntPtr pImgFactory = IntPtr.Zero;
        try
        {
            Guid iid = IID_IShellItemImageFactory;
            SHCreateItemFromParsingName(filePath, IntPtr.Zero, ref iid, out pImgFactory);
            if (pImgFactory == IntPtr.Zero)
                return false;

            IShellItemImageFactory factory = Marshal.GetObjectForIUnknown(pImgFactory) as IShellItemImageFactory;
            Marshal.Release(pImgFactory);
            pImgFactory = IntPtr.Zero;

            SIZE size = new(256, 256);
            IntPtr hBitmap = IntPtr.Zero;
            factory.GetImage(size, SIIGBF.RESIZETOFIT | SIIGBF.BIGGERSIZEOK, out hBitmap);

            if (hBitmap == IntPtr.Zero)
                return false;

            try
            {
                BitmapSource bs = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions( ));

                if (bs.CanFreeze) bs.Freeze( );

                source = bs;
                return true;
            }
            catch { }
            finally
            {
                DeleteObject(hBitmap);
            }
        }
        catch { }
        finally
        {
            if (pImgFactory != IntPtr.Zero)
                Marshal.Release(pImgFactory);
        }
        return false;
    }
}
