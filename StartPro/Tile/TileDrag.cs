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

    private bool postDrag;

    private Vector startMousePoint, startTilePoint;

    protected void TileDragging(object o, MouseEventArgs e)
    {
        if (postDrag)
        {
            postDrag = false;
            return;
        }
        if (e.LeftButton == MouseButtonState.Released)
            return;
        IsDrag = true;
        Vector mousePoint = (Vector) Mouse.GetPosition((Panel) Parent);
        Vector offset = mousePoint - startMousePoint;
        Canvas.SetLeft(o as TileBase, startTilePoint.X + offset.X);
        Canvas.SetTop(o as TileBase, startTilePoint.Y + offset.Y);
    }

    protected void TileDragStart(object o, MouseButtonEventArgs e)
    {
        postDrag = true;
        TileBase tile = o as TileBase;
        startMousePoint = (Vector) Mouse.GetPosition((Panel) Parent);
        startTilePoint = new Vector(Canvas.GetLeft(tile), Canvas.GetTop(tile));
        ToTopmost( );
        CaptureMouse( );
        e.Handled = true;
    }

    protected void TileDragStop(object o, MouseButtonEventArgs e)
    {
        postDrag = true;
        if (!IsDrag)
            return;
        Vector mousePoint = (Vector) Mouse.GetPosition((Panel) Parent);
        Vector offset = mousePoint - startMousePoint;
        Vector tilePoint = startTilePoint + offset;
        (o as TileBase).Column = (int) Math.Round(tilePoint.X / TileDatas.BlockSize);
        (o as TileBase).Row = (int) Math.Round(tilePoint.Y / TileDatas.BlockSize);

        Refresh( );
        ReleaseMouseCapture( );
        IsDrag = false;
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
