using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using StartPro.Api;

namespace StartPro.Tile;

public partial class TileBase
{
    private Point startMousePoint, startTilePoint;

    public static readonly DependencyProperty IsDragProperty =
        DependencyProperty.Register("IsDrag", typeof(bool), typeof(AppTile), new PropertyMetadata(false));

    public bool IsDrag
    {
        get => (bool) GetValue(IsDragProperty);
        set => SetValue(IsDragProperty, value);
    }

    protected bool IsDragged { get; set; }

    protected void TileDragStart(object o, MouseButtonEventArgs e)
    {
        AppTile tile = o as AppTile;
        tile?.CaptureMouse( );
        startMousePoint = e.GetPosition(Parent as Panel);
        startTilePoint = new Point(Canvas.GetLeft(tile), Canvas.GetTop(tile));
        IsDrag = true;
    }

    protected void TileDragStop(object o, MouseButtonEventArgs e)
    {
        IsDrag = IsDragged = false;
        AppTile tile = o as AppTile;
        tile?.ReleaseMouseCapture( );

        Point mousePoint = e.GetPosition(Parent as Panel);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        Point tilePoint = new(startTilePoint.X + offset.X, startTilePoint.Y + offset.Y);
        tile.Column = (int) Math.Round(tilePoint.X / Defaults.BlockSize);
        tile.Row = (int) Math.Round(tilePoint.Y / Defaults.BlockSize);

        MoveToSpace(Parent as Panel, false);
        ReindexTiles(Parent as Panel);
    }

    protected void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsDrag || e.LeftButton == MouseButtonState.Released)
            return;
        AppTile tile = o as AppTile;
        Panel.SetZIndex(tile, int.MaxValue);
        Point mousePoint = e.GetPosition(Parent as Panel);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        if (offset.X != 0 || offset.Y != 0)
            IsDragged = true;
        Canvas.SetLeft(tile, startTilePoint.X + offset.X);
        Canvas.SetTop(tile, startTilePoint.Y + offset.Y);
    }

    private bool IntersectsWith(AppTile t)
    {
        if (this == t)
            return false;
        Rect r1 = new((double) Canvas.GetLeft(this), (double) Canvas.GetTop(this), ActualWidth, ActualHeight);
        Rect r2 = new((double) Canvas.GetLeft(t), (double) Canvas.GetTop(t), t.ActualWidth, t.ActualHeight);
        return r1.IntersectsWith(r2);
    }

    public void MoveToSpace(Panel parent, bool moveSelf)
    {
        bool isIntersect = true;
        while (isIntersect)
        {
            isIntersect = false;
            for (int i = 0; i < parent.Children.Count; i++)
            {
                AppTile target = parent.Children[i] as AppTile;
                if (IntersectsWith(target))
                {
                    Row++;
                    isIntersect = true;
                    break;
                }
            }
        }
        ResizeCanvas(parent);
    }

    private void ReindexTiles(Panel parent)
    {
        for (int i = 0; i < parent.Children.Count; i++)
        {
            Panel.SetZIndex(parent.Children[i], i);
            Panel.SetZIndex(this, parent.Children.Count);
        }
    }

    public static void ResizeCanvas(Panel parent)
    {
        double xmax = 0, ymax = 0;
        foreach (AppTile t in parent.Children)
        {
            double txmax = t.Column * Defaults.BlockSize + t.ActualWidth;
            double tymax = t.Row * Defaults.BlockSize + t.ActualHeight;
            xmax = txmax > xmax ? txmax : xmax;
            ymax = tymax > ymax ? tymax : ymax;
        }
        (parent.Width, parent.Height) = (xmax + Defaults.Margin * 2, ymax + Defaults.Margin * 2);
    }
}
