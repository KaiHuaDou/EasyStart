using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro.Tile;

public partial class TileBase
{
    public static readonly DependencyProperty IsDragProperty =
        DependencyProperty.Register("IsDrag", typeof(bool), typeof(TileBase), new PropertyMetadata(false));

    public bool IsDrag
    {
        get => (bool) GetValue(IsDragProperty);
        set => SetValue(IsDragProperty, value);
    }

    protected bool OnDrag { get; set; }

    private Point startMousePoint, startTilePoint;
    private Point mousePoint => Mouse.GetPosition(Parent as Panel);
    private Point offset => new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
    private Point tilePoint => new(startTilePoint.X + offset.X, startTilePoint.Y + offset.Y);
    protected void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsDrag || e.LeftButton == MouseButtonState.Released)
        {
            OnDrag = false;
            return;
        }
        TileBase tile = o as TileBase;
        Panel.SetZIndex(tile, Owner.Children.Count + 1);
        if (offset.X != 0 || offset.Y != 0)
        {
            OnDrag = true;
            Canvas.SetLeft(tile, startTilePoint.X + offset.X);
            Canvas.SetTop(tile, startTilePoint.Y + offset.Y);
        }
    }

    protected void TileDragStart(object o, MouseButtonEventArgs e)
    {
        IsDrag = true;
        TileBase tile = o as TileBase;
        startMousePoint = e.GetPosition(Parent as Panel);
        startTilePoint = new Point(Canvas.GetLeft(tile), Canvas.GetTop(tile));
    }

    protected void TileDragStop(object o, MouseButtonEventArgs e)
    {
        if (!IsDrag || !OnDrag)
            return;
        IsDrag = false;
        TileBase tile = o as TileBase;

        tile.Column = (int) Math.Round(tilePoint.X / TileDatas.BlockSize);
        tile.Row = (int) Math.Round(tilePoint.Y / TileDatas.BlockSize);

        Refresh( );
        ToTopmost( );
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
        bool isIntersect = true;
        while (isIntersect)
        {
            isIntersect = false;
            for (int i = 0; i < Owner.Children.Count; i++)
            {
                TileBase target = Owner.Children[i] as TileBase;
                if (this != target && IntersectsWith(target))
                {
                    Row++;
                    isIntersect = true;
                    break;
                }
            }
        }
        Owner.ResizeToFit( );
    }

    private void ToTopmost( )
    {
        for (int i = 0; i < Owner.Children.Count; i++)
            Panel.SetZIndex(Owner.Children[i], i);
        Panel.SetZIndex(this, Owner.Children.Count);
    }
}

public static class CanvasExtension
{
    public static void ResizeToFit(this Canvas parent)
    {
        double xmax = 0, ymax = 0;
        foreach (TileBase t in parent.Children)
        {
            double txmax = t.Column * TileDatas.BlockSize + t.ActualWidth;
            double tymax = t.Row * TileDatas.BlockSize + t.ActualHeight;
            xmax = txmax > xmax ? txmax : xmax;
            ymax = tymax > ymax ? tymax : ymax;
        }
        (parent.Width, parent.Height) = (xmax + TileDatas.BaseMargin * 2, ymax + TileDatas.BaseMargin * 2 * 2);
    }
}
