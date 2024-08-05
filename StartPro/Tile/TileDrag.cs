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

    protected bool IsDragged { get; set; }

    public void MoveToSpace( )
    {
        bool isIntersect = true;
        while (isIntersect)
        {
            isIntersect = false;
            for (int i = 0; i < Owner.Children.Count; i++)
            {
                TileBase target = Owner.Children[i] as TileBase;
                if (IntersectsWith(target))
                {
                    Row++;
                    isIntersect = true;
                    break;
                }
            }
        }
        Owner.ResizeToFit( );
    }

    private Point startMousePoint, startTilePoint;

    protected void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsDrag || e.LeftButton == MouseButtonState.Released)
            return;
        TileBase tile = o as TileBase;
        Panel.SetZIndex(tile, int.MaxValue);
        Point mousePoint = e.GetPosition(Parent as Panel);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        if (offset.X != 0 || offset.Y != 0)
            IsDragged = true;
        Canvas.SetLeft(tile, startTilePoint.X + offset.X);
        Canvas.SetTop(tile, startTilePoint.Y + offset.Y);
    }

    protected void TileDragStart(object o, MouseButtonEventArgs e)
    {
        TileBase tile = o as TileBase;
        tile?.CaptureMouse( );
        startMousePoint = e.GetPosition(Parent as Panel);
        startTilePoint = new Point(Canvas.GetLeft(tile), Canvas.GetTop(tile));
        IsDrag = true;
    }

    protected void TileDragStop(object o, MouseButtonEventArgs e)
    {
        IsDrag = IsDragged = false;
        TileBase tile = o as TileBase;
        tile?.ReleaseMouseCapture( );

        Point mousePoint = e.GetPosition(Parent as Panel);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        Point tilePoint = new(startTilePoint.X + offset.X, startTilePoint.Y + offset.Y);
        tile.Column = (int) Math.Round(tilePoint.X / TileDatas.BlockSize);
        tile.Row = (int) Math.Round(tilePoint.Y / TileDatas.BlockSize);

        Refresh( );
        ToTopmost( );
    }
    private bool IntersectsWith(TileBase t)
    {
        if (this == t)
            return false;
        Rect r1 = new((double) Canvas.GetLeft(this), (double) Canvas.GetTop(this), ActualWidth, ActualHeight);
        Rect r2 = new((double) Canvas.GetLeft(t), (double) Canvas.GetTop(t), t.ActualWidth, t.ActualHeight);
        return r1.IntersectsWith(r2);
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
