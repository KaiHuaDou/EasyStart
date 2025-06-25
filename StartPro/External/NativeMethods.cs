using System;
using System.Runtime.InteropServices;

namespace StartPro.External;
internal static partial class NativeMethods
{
    internal const int WM_HOTKEY = 0x312;

    [Flags]
    internal enum KeyModifiers : uint
    {
        None = 0,
        Alt = 1,
        Ctrl = 1 << 1,
        Shift = 1 << 2,
        WindowsKey = 1 << 3
    }

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetForegroundWindow(IntPtr hWnd);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool RegisterHotKey(
        IntPtr hWnd,
        int id,
        KeyModifiers fsModifuers,
        uint vk
    );

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool UnregisterHotKey(IntPtr hWnd, int id);

    [LibraryImport("user32.dll", EntryPoint = "PrivateExtractIconsW", StringMarshalling = StringMarshalling.Utf16)]
    internal static partial int PrivateExtractIcons(
        string lpszFile,
        int nIconIndex,
        int cxIcon,
        int cyIcon,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6), In, Out] IntPtr[] phicon,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6), In, Out] int[] piconid,
        int nIcons,
        int flags
    );

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool DestroyIcon(IntPtr hIcon);
}
