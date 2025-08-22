using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using StartPro.Resources;

namespace StartPro.Api;

public static class Utils
{
    public static readonly string ParentDir = Path.GetDirectoryName(Environment.ProcessPath);

    private static readonly Dictionary<string, (string defaultExt, string filter)> FileFilter = new( ) {
        { "exe", (".exe", "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*") },
        { "img", (".png", "*.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
        { "exe+img", (".png", "*.exe *.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.exe;*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
    };

    public static void AppendContexts(ContextMenu from, ContextMenu to)
    {
        int count = from.Items.Count;
        for (int i = 0; i < count; i++)
        {
            if (from.Items[0] is not MenuItem item)
                continue;
            from.Items.Remove(item);
            to.Items.Add(item);
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

    public static bool CreateBitmapImageSafe(string uri, out BitmapImage image)
    {
        try
        {
            image = new BitmapImage(new Uri(uri));
            return true;
        }
        catch
        {
            image = null;
            return false;
        }
    }

    public static ImageSource ParseImageSource(string text)
    {
        ImageSource result = new BitmapImage( );
        if (text.EndsWith(".exe", StringComparison.InvariantCultureIgnoreCase)
            && PEIcon.FromFile(text, out BitmapSource source))
        {
            result = source;
        }
        // 快捷方式解包交由上层，本层使用 PEIcon.GetComplex 直接获取
        //
        // else if (text.EndsWith(".lnk", StringComparison.InvariantCultureIgnoreCase)
        //         && ResolveShortcut(text, out string target, out _)
        //         && PEIcon.FromFile(target, out BitmapSource source1))
        // {
        //     result = source1;
        // }
        else if (File.Exists(text) && CreateBitmapImageSafe(text, out BitmapImage image))
        {
            result = image;
        }
        else if (PEIcon.GetComplex(text, out BitmapSource thumbnail))
        {
            result = thumbnail;
        }
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
            catch
            {
                brush = null;
                return false;
            }
        }
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
