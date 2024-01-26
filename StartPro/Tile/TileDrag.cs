using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro;

public partial class Tile
{
    public bool IsDrag { get; set; }

    private static Point dStart;
    private static readonly Grid dGrid = (Application.Current.MainWindow as MainWindow).mainGrid;

    private static TileGrid PtrPos
    {
        get
        {
            Point point = Mouse.GetPosition(dGrid);
            int cellSize = Defaults.SmallSize + Defaults.Margin;
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
        SetTilePos(PtrPos, t.TileSize, false);
        t.IsDrag = true;
        dStart = e.GetPosition(this);
        t.CaptureMouse( );
    }
    private void TileDragging(object o, MouseEventArgs e)
    {
        Tile t = o as Tile;
        if (!t.IsDrag) return;
        Vector move = e.GetPosition(this) - dStart;
        t.Margin = new Thickness(t.Margin.Left + move.X,
                                 t.Margin.Top + move.Y,
                                 t.Margin.Right - move.X,
                                 t.Margin.Bottom - move.Y);
    }
    private void TileDragStop(object o, MouseButtonEventArgs e)
    {
        Tile t = o as Tile;
        t.ReleaseMouseCapture( );
        t.IsDrag = false;
        t.Margin = new Thickness(0);
        TileGrid pos = PtrPos;
        if (!IsPosEmpty(pos, t.TileSize)) return;
        t.Row = pos.Row;
        t.Column = pos.Col;
        SetTilePos(pos, t.TileSize, true);
    }
}
