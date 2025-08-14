using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using StartPro.Resources;

namespace StartPro.Api;

public static class Utils
{
    public static Dictionary<string, (string defaultExt, string filter)> FileFilter = new( ) {
        { "exe", (".exe", "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*") },
        { "img", (".png", "*.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
        { "exe+img", (".png", "*.exe *.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.exe;*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
    };

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
            App.ShowInfo("无法以管理员身份运行");
        }
    }
    public static string ReadShortcut(string lnk)
    {
        Type shellType = Type.GetTypeFromProgID("WScript.Shell");
        dynamic shell = Activator.CreateInstance(shellType);
        dynamic shortcut = shell.CreateShortcut(lnk);
        return shortcut.TargetPath;
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
        else if (text.StartsWith('#') && text.Length == 9
            && byte.TryParse(text.AsSpan(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte a)
            && byte.TryParse(text.AsSpan(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte r)
            && byte.TryParse(text.AsSpan(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte g)
            && byte.TryParse(text.AsSpan(7, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out byte b)
        )
        {
            brush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
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

    public static bool TrySelectColor(Color from, out Color result, System.Windows.Window owner)
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
