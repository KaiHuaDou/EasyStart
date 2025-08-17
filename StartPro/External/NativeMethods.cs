using System;
using System.Runtime.InteropServices;

namespace StartPro.External;

internal static class NativeMethods
{
    internal const uint SHGFI_ICON = 0x000000100;

    internal const uint SHGFI_LARGEICON = 0x000000000;

    internal const uint SHGFI_SMALLICON = 0x000000001;

    internal const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

    internal static readonly Guid IID_IShellItemImageFactory = new("bcc18b79-ba16-442f-80c4-8a59c30c463b");

    [Flags]
    internal enum SIIGBF : uint
    {
        RESIZETOFIT = 0x00,
        BIGGERSIZEOK = 0x01,
        MEMORYONLY = 1 << 1,
        ICONONLY = 1 << 2,
        THUMBNAILONLY = 1 << 3,
        INCACHEONLY = 1 << 4
    }

    [ComImport]
    [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IShellItemImageFactory
    {
        void GetImage([In] SIZE size, [In] SIIGBF flags, out IntPtr phbm);
    }

    [DllImport("gdi32.dll")]
    internal static extern bool DeleteObject(IntPtr hObject);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    internal static extern void SHCreateItemFromParsingName(
        [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
        IntPtr pbc,
        [In] ref Guid riid,
        out IntPtr ppv
    );

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct SHFILEINFOW
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SIZE(int x, int y)
    {
        public int cx = x;
        public int cy = y;
    }
}
