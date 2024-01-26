using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

    private void SetGrid(object o, SizeChangedEventArgs e)
    {
        int HeightCnt = (int) Math.Floor(ActualHeight / Defaults.SmallSize);
        int WidthCnt = (int) Math.Floor(ActualWidth / Defaults.SmallSize);
        while (true)
        {
            if (mainGrid.RowDefinitions.Count == HeightCnt) break;
            else if (mainGrid.RowDefinitions.Count < HeightCnt)
                mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Defaults.SmallSize + Defaults.Margin) });
            else mainGrid.RowDefinitions.RemoveAt(0);
        }
        while (true)
        {
            if (mainGrid.ColumnDefinitions.Count == WidthCnt) break;
            else if (mainGrid.ColumnDefinitions.Count < WidthCnt)
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Defaults.SmallSize + Defaults.Margin) });
            else mainGrid.ColumnDefinitions.RemoveAt(0);
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
