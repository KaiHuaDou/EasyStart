using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Input;
using Microsoft.Win32;
using NHotkey;
using NHotkey.Wpf;

namespace StartPro.Api;

public static class Integration
{
    private const string AppName = "EasyStart";

    private static readonly string AppPath = $"\"{Environment.ProcessPath}\"";

    public static bool AddToStartup( )
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (key is null)
            {
                App.AddInfo("添加启动项失败 - 找不到注册表键");
                return false;
            }
            key.SetValue(AppName, AppPath);
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            App.AddInfo("添加启动项失败 - 权限不足");
        }
        catch (IOException)
        {
            App.AddInfo("添加启动项失败 - I/O错误");
        }
        catch (Exception ex)
        {
            App.AddInfo($"添加启动项失败: {ex.Message}");
        }
        return false;
    }
    public static void ExecuteAsAdmin(string executable, string arguments = "")
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent( );
        WindowsPrincipal principal = new(identity);
        bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
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
            App.AddInfo("无法以管理员身份运行");
        }
    }
    public static bool ReadStartup( )
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            if (key is null)
            {
                App.AddInfo("读取启动项失败 - 找不到注册表键");
                return false;
            }
            return key.GetValue(AppName)?.ToString( ) != AppPath;
        }
        catch (Exception ex)
        {
            App.AddInfo($"读取启动项失败 - {ex.Message}");
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
            App.AddInfo("无法注册热键 - 已被占用");
        }
        catch (Exception ex)
        {
            App.AddInfo($"无法注册热键 - {ex.Message}");
        }
    }

    public static bool RemoveFromStartup( )
    {
        try
        {
            using RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (key is null)
            {
                App.AddInfo("删除启动项失败 - 找不到注册表键");
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
            App.AddInfo("删除启动项失败 - 权限不足");
        }
        catch (IOException)
        {
            App.AddInfo("删除启动项失败 - I/O错误");
        }
        catch (Exception ex)
        {
            App.AddInfo($"删除启动项失败: {ex.Message}");
        }
        return false;
    }

    public static bool ResolveShortcut(string lnk, out string target, out string arguments)
    {
        target = null;
        arguments = null;
        dynamic shell = null;
        dynamic shortcut = null;

        try
        {
            if (string.IsNullOrWhiteSpace(lnk) ||
                !File.Exists(lnk) ||
                !Path.GetExtension(lnk).Equals(".lnk", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Type shellType = Type.GetTypeFromProgID("WScript.Shell");
            shell = Activator.CreateInstance(shellType);
            shortcut = shell.CreateShortcut(lnk);

            target = shortcut.TargetPath as string;
            arguments = shortcut.Arguments as string ?? string.Empty;

            return !string.IsNullOrWhiteSpace(target);
        }
        catch (Exception ex)
        {
            App.AddInfo($"解析快捷方式失败 - {ex.Message}");
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
