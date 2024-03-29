﻿using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Media;
using Microsoft.Win32;
using StartPro.Resources;

namespace StartPro.Api;

public static class Utils
{
    public static bool TrySelectImage(out string fileName)
    {
        OpenFileDialog dialog = new( )
        {
            CheckFileExists = true,
            Filter = "*.exe *.dll *.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.exe;*.dll;*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*",
            Title = Main.SelectImageText
        };
        bool result = dialog.ShowDialog( ) == true;
        fileName = dialog.FileName;
        return result;
    }

    public static bool TrySelectColor(out Color color)
    {
        System.Windows.Forms.ColorDialog dialog = new( )
        {
            AllowFullOpen = true,
            AnyColor = true,
        };
        bool result = dialog.ShowDialog( ) == System.Windows.Forms.DialogResult.OK;
        color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
        return result;
    }

    public static bool TrySelectExe(out string fileName)
    {
        OpenFileDialog dialog = new( )
        {
            CheckFileExists = true,
            DefaultExt = ".exe",
            Filter = "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*",
            Title = Main.SelectExeText,
        };
        bool result = dialog.ShowDialog( ) == true;
        fileName = dialog.FileName;
        return result;
    }

    public static bool TrySelectExe(out string[] fileName)
    {
        OpenFileDialog dialog = new( )
        {
            CheckFileExists = true,
            DefaultExt = ".exe",
            Filter = "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*",
            Title = Main.SelectExeText,
            Multiselect = true,
        };
        bool result = dialog.ShowDialog( ) == true;
        fileName = dialog.FileNames;
        return result;
    }

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
        catch { }
    }
    public static string ReadShortcut(string lnk)
    {
        Type shellType = Type.GetTypeFromProgID("WScript.Shell");
        dynamic shell = Activator.CreateInstance(shellType);
        dynamic shortcut = shell.CreateShortcut(lnk);
        return shortcut.TargetPath;
    }
}
