using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        var tile = window.Item;
        if (tile?.IsEnabled != true)
        {
            return;
        }

        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void AddImageTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateImage window = new( );
        window.ShowDialog( );
        Show( );
        var tile = window.Item;
        if (tile?.IsEnabled != true)
        {
            return;
        }

        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void AddTextTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateText window = new( );
        window.ShowDialog( );
        Show( );
        var tile = window.Item;
        if (tile?.IsEnabled != true)
        {
            return;
        }

        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void AddTiles(IEnumerable<TileBase> tiles)
    {
        foreach (var tile in tiles)
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
        foreach (var tile in window.Tiles)
        {
            TilePanel.Children.Add(tile);
            tile.IsEnabled = true;
            tile.Refresh( );
        }

        Show( );
    }

    private async void ImportSystemStart(object o, RoutedEventArgs e)
    {
        ImportSystemStartButton.IsEnabled = false;
        var tileDataList = await Task.Run(SystemTiles.ImportData);
        var tiles = tileDataList?.Select(SystemTiles.CreateTile) ?? [];
        AddTiles(tiles);
        ImportSystemStartButton.IsEnabled = true;
    }

    private void PinApp(object o, RoutedEventArgs e)
    {
        if (o is not MenuItem menuItem || menuItem.CommandParameter is not SystemApp app)
        {
            return;
        }

        AppTile appTile = new( )
        {
            AppName = app.AppName,
            AppPath = app.AppPath,
            AppIcon = app.AppPath,
            TileSize = TileSize.Medium,
            Row = 0,
            Column = 0
        };
        TilePanel.Children.Add(appTile);
        appTile.Refresh( );
    }

    private void SwitchAppList(object? o, RoutedEventArgs? e)
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
        {
            App.Tiles.Add(tile);
        }
    }
}
