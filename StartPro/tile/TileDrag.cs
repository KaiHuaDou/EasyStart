using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro
{
    public partial class Tile
    {
        private Point dragPos;
        private Thickness dragMargin;
        private Grid mainGrid = (App.Current.MainWindow as MainWindow).mainGrid;

        private TileGrid PtrPos
        {
            get
            {
                Point point = Mouse.GetPosition(mainGrid);
                int cellSize = Default.SmallSize + Default.Margin;
                return new TileGrid
                {
                    Row = (int) (uint) Math.Floor(point.Y / cellSize),
                    Col = (int) (uint) Math.Floor(point.X / cellSize)
                };
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
    }
}
