using System.Collections.Generic;
using System.Windows;

namespace StartPro;

public partial class MainWindow : Window
{
    public MainWindow( )
    {
        InitializeComponent( );
        HashSet<Tile> tiles = Tile.Load( );
        foreach (Tile tile in tiles)
        {
            tile.Init( );
            mainGrid.Children.Add(tile);
        }
    }

    private void AddTile(object sender, RoutedEventArgs e)
    {
        Create window = new( );
        window.ShowDialog( );
        Tile tile = window.tile;
        if (!tile.IsEnabled) return;
        mainGrid.Children.Add(tile);
    }

    private void SaveTiles(object sender, System.ComponentModel.CancelEventArgs e)
    {
        foreach (Tile tile in mainGrid.Children)
            Tile.Add(tile);
        Tile.Save( );
    }
}
