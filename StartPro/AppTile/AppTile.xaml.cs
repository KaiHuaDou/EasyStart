using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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

        MouseLeftButtonUp += TileLeftButtonUp;

        userControl.Content = null;
        border.Child = RootPanel;
        Content = root;

        Foreground = Utils.TryParseBrushFromText(App.Settings.Foreground, out Brush fore)
            ? fore : Defaults.Foreground;
    }

    public IEditor<AppTile> Editor => new CreateApp(this);

    protected static void AppIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AppTile tile = o as AppTile;
        string path = e.NewValue as string;
        tile.image.Source = Utils.ParseImageSourceFromText(path);
    }

    protected static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        AppTile tile = o as AppTile;
        tile.AppIcon = e.NewValue as string;
    }

    protected static void ImageShadowChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        (o as AppTile)?.TileImageShadow.Opacity = (!App.Settings.UIFlat && (o as AppTile).ImageShadow) ? 0.4 : 0;
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
            Utils.ExecuteAsAdmin("explorer.exe", $"/e, /select, {AppPath}");
            App.TileWindow.Hide( );
        }
        catch (Win32Exception)
        {
            App.AddInfo("无法打开应用所在目录");
        }
    }

    private void RunAsAdmin(object o, RoutedEventArgs e)
    {
        Utils.ExecuteAsAdmin(AppPath);
        App.TileWindow.Hide( );
    }

    private void TileLeftButtonUp(object o, MouseButtonEventArgs e)
    {
        if (IsDragging || !IsEnabled)
            return;

        try
        {
            Process.Start(new ProcessStartInfo( )
            {
                UseShellExecute = true,
                FileName = AppPath,
            });
        }
        catch (Exception ex)
        {
            App.AddInfo($"无法启动程序: {ex.Message}");
        }
        App.TileWindow.Hide( );
    }
}
