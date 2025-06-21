using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using StartPro.Api;

namespace StartPro.Tile;

public partial class AppTile : TileBase
{
    static AppTile( )
    {
        TileSizeProperty.OverrideMetadata(typeof(AppTile), new(TileSize.Medium, TileSizeChanged));
    }

    protected static new void TileSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        TileBase.TileSizeChanged(o, e);
        AppTile appTile = o as AppTile;
        appTile?.TileLabel.Visibility =
            appTile?.TileSize is TileSize.Small or TileSize.Thin or TileSize.Tall
            ? Visibility.Collapsed : Visibility.Visible;
    }

    protected static void AppIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AppTile tile = o as AppTile;
        string path = e.NewValue as string;
        if (tile.AppIcon.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
            tile.image.Source = PEIcon.Get(path);
        }
        else if (File.Exists(path))
        {
            try
            {
                tile.image.Source = new BitmapImage(new Uri(path));
            }
            catch
            {
                tile.image.Source = new BitmapImage( );
            }
        }
    }

    protected static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AppTile tile = o as AppTile;
        string value = e.NewValue as string;
        FileInfo app = new(value);
        tile.AppName = app.Name.Replace(app.Extension, "");
        tile.AppIcon = value;
    }

    protected static void ImageShadowChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        (o as AppTile)?.TileImageShadow.Opacity = (!App.Settings.Content.UIFlat && (o as AppTile).ImageShadow) ? 0.4 : 0;
    }

    protected static new void TileColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AppTile tile = o as AppTile;
        Color color = tile.TileColor.Color;
        int brightness = (int) (color.R * 0.299) + (int) (color.G * 0.587) + (int) (color.B * 0.114);
        tile.TileLabel.Foreground = new SolidColorBrush(brightness > 128 ? Colors.Black : Colors.White);
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        Panel parent = Parent as Panel;
        parent.Children.Remove(this);
        CreateApp c = new(this);
        c.ShowDialog( );
        c.Item.IsEnabled = true;
        parent.Children.Add(c.Item);
        c.Item.Refresh( );
    }

    private void OpenTileLocation(object o, RoutedEventArgs e)
    {
        try
        {
            Utils.ExecuteAsAdmin(Directory.GetParent(AppPath).FullName);
            App.TileWindow.Hide( );
        }
        catch (Win32Exception ex)
        {
            MessageBox.Show(ex.Message, "StartPro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RunAsAdmin(object o, RoutedEventArgs e)
    {
        Utils.ExecuteAsAdmin(AppPath);
        App.TileWindow.Hide( );
    }

    private new void TileLeftButtonUp(object o, MouseButtonEventArgs e)
    {
        if (!OnDrag && IsEnabled)
        {
            Process.Start(new ProcessStartInfo( )
            {
                UseShellExecute = true,
                FileName = AppPath,
            });
            App.TileWindow.Hide( );
        }
        base.TileLeftButtonUp(o, e);
    }
}
