using System;
using System.Runtime.InteropServices;

namespace StartPro.External;

internal static partial class NativeMethods
{
    internal const uint EWX_FORCE = 0x00000004;
    internal const uint EWX_LOGOFF = 0x00000000;
    internal const uint EWX_REBOOT = 0x00000002;
    internal const uint EWX_SHUTDOWN = 0x00000001;
    internal const uint SE_PRIVILEGE_ENABLED = 0x00000002;
    internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
    internal const uint SHGFI_ICON = 0x000000100;
    internal const uint SHGFI_LARGEICON = 0x000000000;
    internal const uint SHGFI_SMALLICON = 0x000000001;
    internal const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
    internal const uint TOKEN_ADJUST_PRIVILEGES = 0x00000020;
    internal const uint TOKEN_QUERY = 0x00000008;
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

    [DllImport("advapi32.dll", SetLastError = true)]
    private static extern bool AdjustTokenPrivileges(IntPtr hToken,
        bool bDisableAllPrivileges,
        ref TOKEN_PRIVILEGES lpNewState,
        int nSize,
        IntPtr lpPreviousState,
        IntPtr lpReturnLength
    );

    [LibraryImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool DeleteObject(IntPtr hObject);

    internal static bool EnableShutdownPrivilege( )
    {
        bool success = OpenProcessToken(GetCurrentProcess( ), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out nint hToken);
        if (!success)
            return false;
        success = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, out LUID luidShutdown);
        if (!success)
            return false;
        TOKEN_PRIVILEGES tokenPrivileges = new( )
        {
            PrivilegeCount = 1,
            Privileges = new LUID_AND_ATTRIBUTES[1]
        };
        tokenPrivileges.Privileges[0].Luid = luidShutdown;
        tokenPrivileges.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;

        return AdjustTokenPrivileges(hToken, false, ref tokenPrivileges, 0, IntPtr.Zero, IntPtr.Zero);
    }

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ExitWindowsEx(uint uFlags, uint dwReason);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    internal static partial IntPtr GetCurrentProcess( );

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool LockWorkStation( );

    [LibraryImport("advapi32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLuid);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool OpenProcessToken(IntPtr hProcess, uint dwDesiredAccess, out IntPtr hToken);

    [LibraryImport("powrprof.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool SetSuspendState([MarshalAs(UnmanagedType.Bool)] bool hibernate, [MarshalAs(UnmanagedType.Bool)] bool forceCritical, [MarshalAs(UnmanagedType.Bool)] bool disableWakeEvent);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    internal static extern void SHCreateItemFromParsingName(
            [MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            IntPtr pbc,
            [In] ref Guid riid,
            out IntPtr ppv
        );
    internal struct LUID
    {
        internal int HighPart;
        internal uint LowPart;
    }

    internal struct LUID_AND_ATTRIBUTES
    {
        internal uint Attributes;
        internal LUID Luid;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct SHFILEINFOW
    {
        internal IntPtr hIcon;
        internal int iIcon;
        internal uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        internal string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        internal string szTypeName;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct SIZE(int x, int y)
    {
        internal int cx = x;
        internal int cy = y;
    }

    internal struct TOKEN_PRIVILEGES
    {
        internal uint PrivilegeCount;
        internal LUID_AND_ATTRIBUTES[] Privileges;
    }
}
