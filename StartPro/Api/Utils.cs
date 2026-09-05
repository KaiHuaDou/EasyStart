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
    public static readonly string AppPath = Path.GetDirectoryName(Environment.ProcessPath)!;

    private static readonly Dictionary<string, (string defaultExt, string filter)> FileFilter = new( ) {
        { "exe", (".exe", "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*") },
        { "img", (".png", "*.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
        { "exe+img", (".png", "*.exe *.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.exe;*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*") },
    };

    public static void AppendContexts(ContextMenu from, ContextMenu to)
    {
        var count = from.Items.Count;
        for (var i = 0; i < count; i++)
        {
            if (from.Items[0] is not MenuItem item)
            {
                continue;
            }

            from.Items.Remove(item);
            to.Items.Add(item);
        }
    }

    public static bool CreateBitmapImageSafe(string path, out BitmapImage image)
    {
        try
        {
            image = new BitmapImage(new Uri(path));
            return true;
        }
        catch
        {
            image = new BitmapImage( );
            return false;
        }
    }

    public static string ShortenStr(string str, int len = 25)
    {
        return str.Length <= len ? str : $"{str.AsSpan(0, len - 3)}...";
    }

    public static double ParseFontSize(string value)
    {
        return double.TryParse(value, out var result)
                && result > 0
                ? result : Defaults.FontSize;
    }

    public static bool TryParseBrush(string text, out Brush? brush)
    {
        if (TryParseImageSource(text, out var source))
        {
            brush = new ImageBrush(source) { Stretch = Stretch.UniformToFill };
        }
        else if (TryParseColor(text, out var color))
        {
            brush = new SolidColorBrush(color);
        }
        else
        {
            brush = null;
            return false;
        }

        return true;
    }

    public static bool TryParseColor(string text, out Color color)
    {
        if (text.StartsWith('#') && text.Length == 9
            && byte.TryParse(text.AsSpan(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var a)
            && byte.TryParse(text.AsSpan(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var r)
            && byte.TryParse(text.AsSpan(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var g)
            && byte.TryParse(text.AsSpan(7, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
        {
            color = Color.FromArgb(a, r, g, b);
            return true;
        }

        return false;
    }

    public static bool TryParseImageSource(string text, out ImageSource source)
    {
        if (PEIcon.Direct(text, out var source1))
        {
            source = source1;
        }
        else if (File.Exists(text) && CreateBitmapImageSafe(text, out var image))
        {
            source = image;
        }
        else if (PEIcon.Complex(text, out var source2))
        {
            source = source2;
        }
        else
        {
            source = new BitmapImage( );
            return false;
        }

        return true;
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
        var dialog = CreateOpenFileDialog(fileType);
        var result = dialog.ShowDialog( ) == true;
        fileName = dialog.FileName;
        return result;
    }

    public static bool TrySelectFiles(out string[] fileNames, string fileType)
    {
        var dialog = CreateOpenFileDialog(fileType);
        var result = dialog.ShowDialog( ) == true;
        fileNames = dialog.FileNames;
        return result;
    }

    private static OpenFileDialog CreateOpenFileDialog(string fileType)
    {
        return new( )
        {
            CheckFileExists = true,
            DefaultExt = ".exe",
            Filter = FileFilter[fileType].filter,
            Title = Main.SelectExeText,
        };
    }
}
