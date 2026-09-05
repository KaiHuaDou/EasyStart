using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Input;

using Microsoft.Win32;

using NHotkey;
using NHotkey.Wpf;

using StartPro.Resources;

namespace StartPro.Api;

public static class Integration
{
    private const string AppName = "EasyStart";

    private static readonly string AppPath = $"\"{Environment.ProcessPath}\"";

    public static bool AddToStartup( )
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (key is null)
            {
                App.AddInfo(Info.AddStartupFailedRegistry);
                return false;
            }

            key.SetValue(AppName, AppPath);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            App.AddInfo(Info.AddStartupFailedPermission);
        }
        catch (IOException)
        {
            App.AddInfo(Info.AddStartupFailedIO);
        }
        catch (Exception ex)
        {
            App.AddInfo(string.Format(Info.AddStartupFailedException, ex.Message));
        }

        return false;
    }
    public static void ExecuteAsAdmin(string executable, string arguments = "")
    {
        var identity = WindowsIdentity.GetCurrent( );
        WindowsPrincipal principal = new(identity);
        var isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        try
        {
            Process.Start(new ProcessStartInfo( )
            {
                UseShellExecute = true,
                FileName = executable,
                Arguments = arguments,
                Verb = isAdmin ? "" : "runas"
            });
        }
        catch
        {
            App.AddInfo(Info.RunAsAdminFailed);
        }
    }
    public static bool ReadStartup( )
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            if (key is null)
            {
                App.AddInfo(string.Format(Info.ReadStartupFailed, "找不到注册表键"));
                return false;
            }

            return key.GetValue(AppName)?.ToString( ) != AppPath;
        }
        catch (Exception ex)
        {
            App.AddInfo(string.Format(Info.ReadStartupFailed, ex.Message));
            return false;
        }
    }

    public static void RegisterHotkey(Action action)
    {
        try
        {
            HotkeyManager.Current.AddOrReplace(
                "ShowHide",
#if DEBUG
                Key.None, ModifierKeys.Shift | ModifierKeys.Control,
#else
                Key.None, ModifierKeys.Windows | ModifierKeys.Control,
#endif
            (_, e) =>
            {
                action( );
                e.Handled = true;
            }
            );
        }
        catch (HotkeyAlreadyRegisteredException)
        {
            App.AddInfo(Info.HotkeyRegisterFailedInUse);
        }
        catch (Exception ex)
        {
            App.AddInfo(string.Format(Info.HotkeyRegisterFailedException, ex.Message));
        }
    }

    public static bool RemoveFromStartup( )
    {
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (key is null)
            {
                App.AddInfo(Info.DeleteStartupFailedRegistry);
                return false;
            }

            if (key.GetValue(AppName) != null)
            {
                key.DeleteValue(AppName);
            }

            return true;
        }
        catch (UnauthorizedAccessException)
        {
            App.AddInfo(Info.DeleteStartupFailedPermission);
        }
        catch (IOException)
        {
            App.AddInfo(Info.DeleteStartupFailedIO);
        }
        catch (Exception ex)
        {
            App.AddInfo(string.Format(Info.DeleteStartupFailedException, ex.Message));
        }

        return false;
    }

    public static bool ResolveShortcut(string lnk, out string target, out string arguments)
    {
        target = "";
        arguments = "";
        dynamic shell = null!;
        dynamic shortcut = null!;

        try
        {
            if (string.IsNullOrWhiteSpace(lnk) ||
                !File.Exists(lnk) ||
                !Path.GetExtension(lnk).Equals(".lnk", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var shellType = Type.GetTypeFromProgID("WScript.Shell");
            if (shellType is null)
            {
                return false;
            }

            shell = Activator.CreateInstance(shellType)!;
            shortcut = shell.CreateShortcut(lnk);

            target = shortcut.TargetPath as string ?? throw new InvalidOperationException( );
            arguments = shortcut.Arguments as string ?? string.Empty;

            return !string.IsNullOrWhiteSpace(target);
        }
        catch (Exception ex)
        {
            App.AddInfo(string.Format(Info.ParseShortcutFailed, ex.Message));
            return false;
        }
        finally
        {
            if (shortcut is not null)
            {
                try { Marshal.ReleaseComObject(shortcut); } catch { }
            }

            if (shell is not null)
            {
                try { Marshal.ReleaseComObject(shell); } catch { }
            }
        }
    }
}
