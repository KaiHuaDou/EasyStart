using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro
{

    public partial class MainWindow : Window
    {
        private Point dragPos;
        private Thickness dragMargin;

        public MainWindow( )
        {
            InitializeComponent( );
            HashSet<Tile> tiles = TilesConfig.Load( );
            foreach (Tile tile in tiles)
            {
                InitTile(tile);
                mainGrid.Children.Add(tile);
            }
        }

        private void SetGrid(object o, SizeChangedEventArgs e)
        {
            int HeightCnt = (int) Math.Floor(this.ActualHeight / Default.SmallSize);
            int WidthCnt = (int) Math.Floor(this.ActualWidth / Default.SmallSize);
            while (true)
            {
                if (mainGrid.RowDefinitions.Count == HeightCnt) break;
                else if (mainGrid.RowDefinitions.Count < HeightCnt)
                    mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Default.SmallSize + Default.Margin) });
                else mainGrid.RowDefinitions.RemoveAt(0);
            }
            while (true)
            {
                if (mainGrid.ColumnDefinitions.Count == WidthCnt) break;
                else if (mainGrid.ColumnDefinitions.Count < WidthCnt)
                    mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Default.SmallSize + Default.Margin) });
                else mainGrid.ColumnDefinitions.RemoveAt(0);
            }
        }

        private void TileDragStart(object o, MouseButtonEventArgs e)
        {
            Tile c = o as Tile;
            TileGrid pos = PtrPos;
            Tile.SetTilePos(pos, c.TileSize, false);
            Tile.IsDrag = true;
            dragPos = e.GetPosition(this);
            dragMargin = c.Margin;
            c.CaptureMouse( );
        }
        private void TileDragging(object o, MouseEventArgs e)
        {
            if (!Tile.IsDrag) return;
            var pos = e.GetPosition(this);
            var dp = pos - dragPos;
            Tile c = o as Tile;
            c.Margin = new Thickness(dragMargin.Left + dp.X, dragMargin.Top + dp.Y, dragMargin.Right - dp.X, dragMargin.Bottom - dp.Y);
        }
        private void TileDragStop(object o, MouseButtonEventArgs e)
        {
            Tile.IsDrag = false;
            Tile c = o as Tile;
            c.Margin = new Thickness(0);
            c.ReleaseMouseCapture( );
            TileGrid pos = PtrPos;
            if (!Tile.IsPosEmpty(pos, c.TileSize)) return;
            Grid.SetRow(c, pos.Row);
            Grid.SetColumn(c, pos.Col);
            Tile.SetTilePos(pos, c.TileSize, true);
        }

        private void InitTile(Tile tile)
        {
            Tile.SetTilePos(tile.Pos, tile.TileSize, true);
            tile.MouseRightButtonDown += TileDragStart;
            tile.MouseMove += TileDragging;
            tile.MouseRightButtonUp += TileDragStop;
        }

        private TileGrid PtrPos
        {
            get
            {
                Point point = Mouse.GetPosition(mainGrid);
                int cellSize = Default.SmallSize + Default.Margin;
                return new TileGrid
                {
                    Row = (int) Math.Floor(point.Y / cellSize),
                    Col = (int) Math.Floor(point.X / cellSize)
                };
            }
        }

        private void AddTile(object sender, RoutedEventArgs e)
        {
            Add window = new Add( );
            window.ShowDialog( );
            Tile tile = window.tile;
            if (!tile.IsEnabled) return;
            InitTile(tile);
            mainGrid.Children.Add(tile);
        }

        private void SaveTiles(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Tile tile in mainGrid.Children)
                TilesConfig.Add(tile);
            TilesConfig.Save( );
        }
    }
}
