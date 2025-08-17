using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using NHotkey;
using NHotkey.Wpf;
using StartPro.Resources;

namespace StartPro.Api;

public static class Utils
{
    public static Dictionary<string, (string defaultExt, string filter)> FileFilter = new( ) {
        { "exe", (".exe", "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*") },
        { "img", (".png", "*.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
        { "exe+img", (".png", "*.exe *.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.exe;*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
    };

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

    public static void AppendContexts(ContextMenu from, ContextMenu to)
    {
        int count = from.Items.Count;
        for (int i = 0; i < count; i++)
        {
            MenuItem item = from.Items[0] as MenuItem;
            from.Items.Remove(item);
            to.Items.Add(item);
        }
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

    public static bool ParseColorFromText(string text, out Color color)
    {
        if (text.StartsWith('#') && text.Length == 9
            && byte.TryParse(text.AsSpan(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte a)
            && byte.TryParse(text.AsSpan(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte r)
            && byte.TryParse(text.AsSpan(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte g)
            && byte.TryParse(text.AsSpan(7, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte b))
        {
            color = Color.FromArgb(a, r, g, b);
            return true;
        }
        return false;
    }

    public static ImageSource ParseImageSourceFromText(string text)
    {
        ImageSource result = new BitmapImage( );
        if (text.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)
            && PEIcon.FromFile(text, out BitmapSource source))
        {
            result = source;
        }
        else if (text.EndsWith(".lnk", StringComparison.InvariantCultureIgnoreCase)
                && PEIcon.FromFile(ResolveShortcut(text), out BitmapSource source1))
        {
            result = source1;
        }
        else
        {
            try
            {
                result = new BitmapImage(new Uri(text));
            }
            catch
            {
                if (PEIcon.GetThumbnail(text, out BitmapSource thumbnail))
                    result = thumbnail;
            }
        }
        return result;
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
            return key.GetValue(AppName).ToString( ) != AppPath;
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

    public static string ResolveShortcut(string lnk)
    {
        try
        {
            Type shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(shellType);
            dynamic shortcut = shell.CreateShortcut(lnk);
            return shortcut.TargetPath;
        }
        catch
        {
            App.AddInfo($"快捷方式解析失败 - {lnk}");
            return null;
        }
    }

    public static string ShortenStr(string str, int len = 25)
    {
        return str.Length <= len ? str : $"{str.AsSpan(0, len - 3)}...";
    }

    public static double ToFontSize(string value)
    {
        return double.TryParse(value, out double result)
                && result > 0
                ? result : Defaults.FontSize;
    }

    public static bool TryParseBrushFromText(string text, out Brush brush)
    {
        if (text.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
            && PEIcon.FromFile(text, out BitmapSource source))
        {
            brush = new ImageBrush(source) { Stretch = Stretch.UniformToFill };
            return true;
        }
        else if (ParseColorFromText(text, out Color color))
        {
            brush = new SolidColorBrush(color);
            return true;
        }
        else
        {
            try
            {
                brush = new ImageBrush(new BitmapImage(new Uri(text))) { Stretch = Stretch.UniformToFill };
                return true;
            }
            catch { }
        }

        brush = null;
        return false;
    }
    public static bool TrySelectColor(Color from, out Color result, Window owner)
    {
        ColorDialog dialog = new( ) { Owner = owner, Color = from };
        dialog.ShowDialog( );
        if (dialog.IsSelected)
        {
            result = dialog.Color;
            return true;
        }
        return false;
    }

    public static bool TrySelectFile(out string fileName, string fileType)
    {
        OpenFileDialog dialog = CreateOpenFileDialog(fileType);
        bool result = dialog.ShowDialog( ) == true;
        fileName = dialog.FileName;
        return result;
    }

    public static bool TrySelectFiles(out string[] fileNames, string fileType)
    {
        OpenFileDialog dialog = CreateOpenFileDialog(fileType);
        bool result = dialog.ShowDialog( ) == true;
        fileNames = dialog.FileNames;
        return result;
    }

    private static OpenFileDialog CreateOpenFileDialog(string fileType) => new( )
    {
        CheckFileExists = true,
        DefaultExt = ".exe",
        Filter = FileFilter[fileType].filter,
        Title = Main.SelectExeText,
    };
}
