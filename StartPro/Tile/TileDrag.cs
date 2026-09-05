using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DependencyPropertyGenerator;

namespace StartPro.Tile;

[DependencyProperty<bool>("IsDragging", DefaultValue = false)]
[DependencyProperty<bool>("IsMouseLeftButtonDown", DefaultValue = false)]
public partial class TileBase
{
    private Vector startMousePoint, startTilePoint;

    protected void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsMouseLeftButtonDown)
        {
            return;
        }

        var mousePoint = Mouse.GetPosition(Owner);
        var offset = mousePoint - startMousePoint;
        if (offset.X == 0 || offset.Y == 0)
        {
            return;
        }

        IsDragging = true;
        Canvas.SetLeft(o as TileBase, startTilePoint.X + offset.X);
        Canvas.SetTop(o as TileBase, startTilePoint.Y + offset.Y);
        e.Handled = true;
    }

    protected void TileDragStart(object o, MouseButtonEventArgs e)
    {
        IsMouseLeftButtonDown = true;

        var tile = (TileBase) o;
        startMousePoint = (Vector) Mouse.GetPosition(Owner);
        startTilePoint = new Vector(Canvas.GetLeft(tile), Canvas.GetTop(tile));

        ToTopmost( );
        CaptureMouse( );
        e.Handled = true;
    }

    protected void TileDragStop(object o, MouseButtonEventArgs e)
    {
        IsMouseLeftButtonDown = false;

        var tile = (TileBase) o;
        startMousePoint = (Vector) Mouse.GetPosition(Owner);
        startTilePoint = new Vector(Canvas.GetLeft(tile), Canvas.GetTop(tile));

        if (!IsDragging)
        {
            return;
        }

        IsDragging = false;
        ReleaseMouseCapture( );

        var mousePoint = (Vector) Mouse.GetPosition(Owner);
        var offset = mousePoint - startMousePoint;
        var tilePoint = startTilePoint + offset;
        tile.Column = (int) Math.Round(tilePoint.X / TileDatas.BlockSize);
        tile.Row = (int) Math.Round(tilePoint.Y / TileDatas.BlockSize);
        Refresh( );

        e.Handled = true;
    }

    private bool IntersectsWith(TileBase t)
    {
        Vector thisOffset = new((double) Canvas.GetLeft(this), (double) Canvas.GetTop(this));
        Vector tOffset = new((double) Canvas.GetLeft(t), (double) Canvas.GetTop(t));
        Rect r1 = new(thisOffset.X, thisOffset.Y, ActualWidth, ActualHeight);
        Rect r2 = new(tOffset.X, tOffset.Y, t.ActualWidth, t.ActualHeight);
        return r1.IntersectsWith(r2);
    }

    private void MoveToSpace( )
    {
        if (Owner is not Panel owner)
        {
            return;
        }

        var isIntersect = true;
        while (isIntersect)
        {
            isIntersect = false;
            for (var i = 0; i < owner.Children.Count; i++)
            {
                if (owner.Children[i] is TileBase target
                    && this != target && IntersectsWith(target))
                {
                    Row++;
                    isIntersect = true;
                    break;
                }
            }
        }
    }

    private void ToTopmost( )
    {
        if (Owner is not Panel owner)
        {
            return;
        }

        for (var i = 0; i < owner.Children.Count; i++)
        {
            Panel.SetZIndex(owner.Children[i], i);
        }

        Panel.SetZIndex(this, owner.Children.Count);
    }
}

public static class PanelExtension
{
    public static void ResizeToFit(this Panel parent)
    {
        double xmax = 0, ymax = 0;
        foreach (TileBase tile in parent.Children)
        {
            var txmax = tile.Column * TileDatas.BlockSize + tile.ActualWidth;
            var tymax = tile.Row * TileDatas.BlockSize + tile.ActualHeight;
            xmax = txmax > xmax ? txmax : xmax;
            ymax = tymax > ymax ? tymax : ymax;
        }

        (parent.Width, parent.Height) = (xmax + TileDatas.BaseMargin * 2, ymax + TileDatas.BaseMargin * 2 * 2);
    }
}
