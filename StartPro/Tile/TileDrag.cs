using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro.Tile;

public partial class TileBase
{
    public static readonly DependencyProperty IsDraggingProperty =
        DependencyProperty.Register("IsDragging", typeof(bool), typeof(TileBase), new PropertyMetadata(false));

    public static readonly DependencyProperty IsMouseLeftButtonDownProperty =
        DependencyProperty.Register("IsMouseLeftButtonDown", typeof(bool), typeof(TileBase), new PropertyMetadata(false));

    public bool IsDragging
    {
        get => (bool) GetValue(IsDraggingProperty);
        set => SetValue(IsDraggingProperty, value);
    }

    public bool IsMouseLeftButtonDown
    {
        get => (bool) GetValue(IsMouseLeftButtonDownProperty);
        set => SetValue(IsMouseLeftButtonDownProperty, value);
    }

    private Vector startMousePoint, startTilePoint;

    protected void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsMouseLeftButtonDown)
            return;

        Point mousePoint = Mouse.GetPosition(Owner);
        Point offset = mousePoint - startMousePoint;
        if (offset.X == 0 || offset.Y == 0)
            return;

        IsDragging = true;
        Canvas.SetLeft(o as TileBase, startTilePoint.X + offset.X);
        Canvas.SetTop(o as TileBase, startTilePoint.Y + offset.Y);
        e.Handled = true;
    }

    protected void TileDragStart(object o, MouseButtonEventArgs e)
    {
        IsMouseLeftButtonDown = true;

        TileBase tile = o as TileBase;
        startMousePoint = (Vector) Mouse.GetPosition(Owner);
        startTilePoint = new Vector(Canvas.GetLeft(tile), Canvas.GetTop(tile));

        ToTopmost( );
        CaptureMouse( );
        e.Handled = true;
    }

    protected void TileDragStop(object o, MouseButtonEventArgs e)
    {
        IsMouseLeftButtonDown = false;

        TileBase tile = o as TileBase;
        startMousePoint = (Vector) Mouse.GetPosition(Owner);
        startTilePoint = new Vector(Canvas.GetLeft(tile), Canvas.GetTop(tile));

        if (!IsDragging)
            return;
        IsDragging = false;
        ReleaseMouseCapture( );

        Vector mousePoint = (Vector) Mouse.GetPosition(Owner);
        Vector offset = mousePoint - startMousePoint;
        Vector tilePoint = startTilePoint + offset;
        (o as TileBase).Column = (int) Math.Round(tilePoint.X / TileDatas.BlockSize);
        (o as TileBase).Row = (int) Math.Round(tilePoint.Y / TileDatas.BlockSize);
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
        bool isIntersect = true;
        while (isIntersect)
        {
            isIntersect = false;
            for (int i = 0; i < Owner.Children.Count; i++)
            {
                if (Owner.Children[i] is TileBase target
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
        for (int i = 0; i < Owner.Children.Count; i++)
            Panel.SetZIndex(Owner.Children[i], i);
        Panel.SetZIndex(this, Owner.Children.Count);
    }
}

public static class PanelExtension
{
    public static void ResizeToFit(this Panel parent)
    {
        double xmax = 0, ymax = 0;
        foreach (TileBase tile in parent.Children)
        {
            double txmax = tile.Column * TileDatas.BlockSize + tile.ActualWidth;
            double tymax = tile.Row * TileDatas.BlockSize + tile.ActualHeight;
            xmax = txmax > xmax ? txmax : xmax;
            ymax = tymax > ymax ? tymax : ymax;
        }
        (parent.Width, parent.Height) = (xmax + TileDatas.BaseMargin * 2, ymax + TileDatas.BaseMargin * 2 * 2);
    }
}
