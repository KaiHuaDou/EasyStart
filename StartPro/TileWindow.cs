using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using StartPro.Api;
using StartPro.Tile;

namespace StartPro;

public partial class MainWindow
{
    private void AddAppTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateApp window = new( );
        window.ShowDialog( );
        Show( );
        AppTile tile = window.Item;
        if (tile?.IsEnabled != true)
            return;
        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void AddImageTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateImage window = new( );
        window.ShowDialog( );
        Show( );
        ImageTile tile = window.Item;
        if (tile?.IsEnabled != true)
            return;
        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void AddTextTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateText window = new( );
        window.ShowDialog( );
        Show( );
        TextTile tile = window.Item;
        if (tile?.IsEnabled != true)
            return;
        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void ImportAppTile(object o, RoutedEventArgs e)
    {
        Hide( );
        ImportApp window = new( );
        window.ShowDialog( );
        foreach (AppTile tile in window.Tiles)
        {
            TilePanel.Children.Add(tile);
            tile.IsEnabled = true;
            tile.Refresh( );
        }
        Show( );
    }

    private void PinApp(object o, RoutedEventArgs e)
    {
        StartMenuApp app = AppList.SelectedItem as StartMenuApp;
        AppTile appTile = new( )
        {
            AppName = app?.AppName,
            AppPath = app?.AppPath,
            AppIcon = app?.AppPath,
            TileSize = TileSize.Medium,
            Row = 0,
            Column = 0
        };
        TilePanel.Children.Add(appTile);
        appTile.Refresh( );
    }

    private void ShowHideAppList(object o, RoutedEventArgs e)
    {
        if ((AppListBorder.RenderTransform as TranslateTransform)?.X != 0 && Resources["ShowAppList"] is Storyboard showAnimation)
        {
            AppListBorder.Visibility = Visibility.Visible;
            showAnimation.Begin(AppListBorder);
        }
        else if (AppListBorder.RenderTransform is TranslateTransform { X: 0 } && Resources["HideAppList"] is Storyboard hideAnimation)
        {
            hideAnimation.Completed += (o, e) => AppListBorder.Visibility = Visibility.Collapsed;
            hideAnimation.Begin(AppListBorder);
        }
    }
}
