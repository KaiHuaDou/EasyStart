using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using StartPro.Resources;

namespace StartPro.Api;

public static class Utils
{
    public static void ExecuteAsAdmin(string executable)
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
                Verb = isAdmin ? "" : "runas"
            });
        }
        catch
        {
            App.ShowInfo("无法以管理员身份运行");
        }
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

    public static string ReadShortcut(string lnk)
    {
        Type shellType = Type.GetTypeFromProgID("WScript.Shell");
        dynamic shell = Activator.CreateInstance(shellType);
        dynamic shortcut = shell.CreateShortcut(lnk);
        return shortcut.TargetPath;
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

    public static Dictionary<string, (string defaultExt, string filter)> FileFilter = new( ) {
        { "exe", (".exe", "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*") },
        { "img", (".png", "*.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
        { "exe+img", (".png", "*.exe *.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.exe;*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
    };

    private static OpenFileDialog CreateOpenFileDialog => new( )
    {
        CheckFileExists = true,
        DefaultExt = ".exe",
        Filter = FileFilter["exe"].filter,
        Title = Main.SelectExeText,
    };

    public static bool TrySelectFile(out string fileName, string fileType)
    {
        OpenFileDialog dialog = CreateOpenFileDialog;
        bool result = dialog.ShowDialog( ) == true;
        fileName = dialog.FileName;
        return result;
    }

    public static bool TrySelectFiles(out string[] fileNames, string fileType)
    {
        OpenFileDialog dialog = CreateOpenFileDialog;
        bool result = dialog.ShowDialog( ) == true;
        fileNames = dialog.FileNames;
        return result;
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
}
