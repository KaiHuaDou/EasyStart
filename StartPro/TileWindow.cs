using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

    private void AddTiles(List<TileBase> tiles)
    {
        foreach (TileBase tile in tiles)
        {
            TilePanel.Children.Add(tile);
            tile.Refresh( );
        }
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

    private async void ImportSystemStart(object o, RoutedEventArgs e)
    {
        List<SystemTiles.TileData> tileDataList = await Task.Run(SystemTiles.ImportData);
        List<TileBase> tiles = tileDataList.ConvertAll(SystemTiles.CreateTile);
        AddTiles(tiles);
    }

    private void PinApp(object o, RoutedEventArgs e)
    {
        if (o is not MenuItem menuItem || menuItem.CommandParameter is not SystemApp app)
            return;
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

    private void SwitchAppList(object o, RoutedEventArgs e)
    {
        AppListScroll.Visibility = AppListSwitchButton.IsChecked == false
            ? Visibility.Collapsed
            : Visibility.Visible;
        TilePanelScroll.Visibility = AppListSwitchButton.IsChecked == true
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    private void SyncGlobalTiles( )
    {
        App.Tiles.Clear( );
        foreach (TileBase tile in TilePanel.Children)
            App.Tiles.Add(tile);
    }
}
