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

public partial class AppTile : TileBase, IEditable<AppTile>
{
    static AppTile( )
    {
        TileSizeProperty.OverrideMetadata(typeof(AppTile), new(TileSize.Medium, TileSizeChanged));
    }

    public AppTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        Utils.AppendContexts(ContextMenu, contextMenu);

        MouseLeftButtonUp -= TileDragStop;
        MouseLeftButtonUp += TileLeftButtonUp;
        MouseLeftButtonUp += TileDragStop;

        userControl.Content = null;
        border.Child = RootPanel;
        Content = root;
    }
    public IEditor<AppTile> Editor => new CreateApp(this);

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
        tile.AppIcon = value;
        if (!tile.IsEnabled)
        {
            FileInfo app = new(value);
            tile.AppName = app.Name.Replace(app.Extension, "");
        }
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
        (tile.TileLabel.Foreground as SolidColorBrush).Color = brightness > 128 ? Colors.Black : Colors.White;
    }

    protected static new void TileSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        TileBase.TileSizeChanged(o, e);
        AppTile appTile = o as AppTile;
        appTile?.TileLabel.Visibility =
            appTile?.TileSize is TileSize.Small or TileSize.Thin or TileSize.Tall
            ? Visibility.Collapsed : Visibility.Visible;
    }
    private void EditTile(object o, RoutedEventArgs e)
    {
        (this as IEditable<AppTile>).Edit(Parent as Panel);
    }

    private void OpenTileLocation(object o, RoutedEventArgs e)
    {
        try
        {
            Utils.ExecuteAsAdmin(Directory.GetParent(AppPath).FullName);
            App.TileWindow.Hide( );
        }
        catch (Win32Exception)
        {
            App.ShowInfo("无法打开应用所在目录");
        }
    }

    private void RunAsAdmin(object o, RoutedEventArgs e)
    {
        Utils.ExecuteAsAdmin(AppPath);
        App.TileWindow.Hide( );
    }

    private void TileLeftButtonUp(object o, MouseButtonEventArgs e)
    {
        if (!IsDrag && IsEnabled)
        {
            Process.Start(new ProcessStartInfo( )
            {
                UseShellExecute = true,
                FileName = AppPath,
            });
            App.TileWindow.Hide( );
        }
    }
}
