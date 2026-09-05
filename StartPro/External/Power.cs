using Windows.Win32.Foundation;
using Windows.Win32.Security;

using static Windows.Win32.PInvoke;

namespace StartPro.External;

internal static class Power
{
    private const string SeShutdownPrivilege = "SeShutdownPrivilege";

    internal static bool EnableShutdownPrivilege( )
    {
        unsafe
        {
            HANDLE hToken;
            if (!OpenProcessToken(GetCurrentProcess( ),
                    TOKEN_ACCESS_MASK.TOKEN_ADJUST_PRIVILEGES | TOKEN_ACCESS_MASK.TOKEN_QUERY,
                    &hToken))
            {
                return false;
            }

            if (!LookupPrivilegeValue(null, SeShutdownPrivilege, out var luidShutdown))
            {
                return false;
            }

            TOKEN_PRIVILEGES tokenPrivileges = new( )
            {
                PrivilegeCount = 1,
            };
            tokenPrivileges.Privileges[0] = new LUID_AND_ATTRIBUTES
            {
                Luid = luidShutdown,
                Attributes = TOKEN_PRIVILEGES_ATTRIBUTES.SE_PRIVILEGE_ENABLED,
            };

            return AdjustTokenPrivileges(hToken, false, &tokenPrivileges,
                (uint) sizeof(TOKEN_PRIVILEGES), null, null);
        }
    }
}
