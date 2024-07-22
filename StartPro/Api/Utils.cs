using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
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
        catch { }
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
}

public static class FastCopy<T>
{
    private static readonly Func<T, T> cache = GetFunc( );

    public static T Copy(T item) => cache(item);

    private static Func<T, T> GetFunc( )
    {
        ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
        List<MemberBinding> memberBindingList = [];

        foreach (PropertyInfo item in typeof(T).GetProperties( ))
        {
            if (!item.CanWrite)
                continue;
            MemberExpression property = Expression.Property(parameterExpression, typeof(T).GetProperty(item.Name));
            MemberBinding memberBinding = Expression.Bind(item, property);
            memberBindingList.Add(memberBinding);
        }

        MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(T)), [.. memberBindingList]);
        Expression<Func<T, T>> lambda = Expression.Lambda<Func<T, T>>(memberInitExpression, [parameterExpression]);

        return lambda.Compile( );
    }
}
