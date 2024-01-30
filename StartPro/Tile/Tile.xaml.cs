using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace StartPro;

public partial class Tile : UserControl
{
    private ProcessStartInfo exec = new(Defaults.AppName);

    public static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        Tile tile = o as Tile;
        string newValue = e.NewValue as string;
        FileInfo app = new(newValue);
        tile.exec = new ProcessStartInfo(newValue);
        tile.AppName = app.Name.Replace(app.Extension, "");
        tile.AppIcon = newValue;

    }

    private static void AppIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        Tile tile = o as Tile;
        string path = e.NewValue as string;
        if (tile.AppIcon == tile.AppName)
            tile.image.Source = IconMgr.Get(path);
        else if (File.Exists(path))
        {
            try { tile.image.Source = new BitmapImage(new Uri(path)); }
            catch { tile.image.Source = IconMgr.Get(path); }
            return;
        }
        tile.image.Source = new BitmapImage( );
    }

    private void TileLeftButtonUp(object o, MouseButtonEventArgs e)
    {
        if (!IsDragged && IsEnabled)
            Process.Start(exec);
        TileDragStop(o, e);
    }
}
