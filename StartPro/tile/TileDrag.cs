using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro
{
    public partial class Tile
    {
        private static Point dPoint;
        private static Thickness dPos;
        private static Grid dGrid = (App.Current.MainWindow as MainWindow).mainGrid;

        private TileGrid PtrPos
        {
            get
            {
                Point point = Mouse.GetPosition(dGrid);
                int cellSize = Default.SmallSize + Default.Margin;
                return new TileGrid
                {
                    Row = (int) Math.Abs(Math.Floor(point.Y / cellSize)),
                    Col = (int) Math.Abs(Math.Floor(point.X / cellSize))
                };
            }
        }

        private void TileDragStart(object o, MouseButtonEventArgs e)
        {
            Tile t = o as Tile;
            TileGrid pos = PtrPos;
            Tile.SetTilePos(pos, t.TileSize, false);
            t.IsDrag = true;
            dPoint = e.GetPosition(this);
            dPos = t.Margin;
            t.CaptureMouse( );
        }
        private void TileDragging(object o, MouseEventArgs e)
        {
            Tile t = o as Tile;
            if (!t.IsDrag) return;
            var pos = e.GetPosition(this);
            var dp = pos - dPoint;
            t.Margin = new Thickness(dPos.Left + dp.X, dPos.Top + dp.Y, dPos.Right - dp.X, dPos.Bottom - dp.Y);
        }
        private void TileDragStop(object o, MouseButtonEventArgs e)
        {
            Tile t = o as Tile;
            t.IsDrag = false;
            t.Margin = new Thickness(0);
            t.ReleaseMouseCapture( );
            TileGrid pos = PtrPos;
            if (!Tile.IsPosEmpty(pos, t.TileSize)) return;
            Grid.SetRow(t, pos.Row);
            Grid.SetColumn(t, pos.Col);
            Tile.SetTilePos(pos, t.TileSize, true);
        }
    }
}
