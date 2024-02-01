using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro;

public partial class Tile
{
    private Point startMousePoint, startTilePoint;

    public static readonly DependencyProperty IsDragProperty =
        DependencyProperty.Register("IsDrag", typeof(bool), typeof(Tile), new PropertyMetadata(false));

    public bool IsDrag
    {
        get => (bool) GetValue(IsDragProperty);
        set => SetValue(IsDragProperty, value);
    }

    private bool IsDragged;

    private void TileDragStart(object o, MouseButtonEventArgs e)
    {
        Tile tile = o as Tile;
        tile?.CaptureMouse( );
        startMousePoint = e.GetPosition(Parent as Panel);
        startTilePoint = new Point(Canvas.GetLeft(tile), Canvas.GetTop(tile));
        IsDrag = true;
    }

    private void TileDragStop(object o, MouseButtonEventArgs e)
    {
        IsDrag = IsDragged = false;
        Tile tile = o as Tile;
        tile?.ReleaseMouseCapture( );

        Point mousePoint = e.GetPosition(Parent as Panel);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        Point tilePoint = new(startTilePoint.X + offset.X, startTilePoint.Y + offset.Y);
        tile.Column = (int) Math.Round(tilePoint.X / Defaults.BlockSize);
        tile.Row = (int) Math.Round(tilePoint.Y / Defaults.BlockSize);

        MoveToSpace(Parent as Panel, offset.X > offset.Y);
        ReindexTiles(Parent as Panel);
    }

    private void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsDrag || e.LeftButton == MouseButtonState.Released)
            return;
        Tile tile = o as Tile;
        Panel.SetZIndex(tile, int.MaxValue);
        Point mousePoint = e.GetPosition(Parent as Panel);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        if (offset.X != 0 || offset.Y != 0)
            IsDragged = true;
        Canvas.SetLeft(tile, startTilePoint.X + offset.X);
        Canvas.SetTop(tile, startTilePoint.Y + offset.Y);
    }

    public bool IntersectsWith(Tile t)
    {
        if (this == t)
            return false;
        Rect r1 = new((double) Canvas.GetLeft(this), (double) Canvas.GetTop(this), ActualWidth, ActualHeight);
        Rect r2 = new((double) Canvas.GetLeft(t), (double) Canvas.GetTop(t), t.ActualWidth, t.ActualHeight);
        return r1.IntersectsWith(r2);
    }

    public void MoveToSpace(Panel parent, bool moveByRow)
    {
        bool flag = true;
        while (flag)
        {
            flag = false;
            for (int i = 0; i < parent.Children.Count; i++)
            {
                if (IntersectsWith(parent.Children[i] as Tile))
                {
                    _ = moveByRow ? Row++ : Column++;
                    flag = true;
                    break;
                }
            }
        }
        ResizeCanvas(parent);
    }

    public void ReindexTiles(Panel parent)
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
        foreach (Tile t in parent.Children)
        {
            double txmax = t.Column * Defaults.BlockSize + t.ActualWidth;
            double tymax = t.Row * Defaults.BlockSize + t.ActualHeight;
            xmax = txmax > xmax ? txmax : xmax;
            ymax = tymax > ymax ? tymax : ymax;
        }
        (parent.Width, parent.Height) = (xmax + Defaults.Margin * 2, ymax + Defaults.Margin * 2);
    }
}
