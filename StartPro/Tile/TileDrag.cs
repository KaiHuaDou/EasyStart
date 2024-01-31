using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro;

public partial class Tile
{
    private static readonly Canvas dGrid = (Application.Current.MainWindow as MainWindow).mainGrid;
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
        startMousePoint = e.GetPosition(dGrid);
        startTilePoint = new Point(Canvas.GetLeft(tile), Canvas.GetTop(tile));
        IsDrag = true;
    }

    private void TileDragStop(object o, MouseButtonEventArgs e)
    {
        IsDrag = IsDragged = false;
        Tile tile = o as Tile;
        tile?.ReleaseMouseCapture( );

        Point mousePoint = e.GetPosition(dGrid);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        Point tilePoint = new(startTilePoint.X + offset.X, startTilePoint.Y + offset.Y);
        tile.Column = (int) Math.Round(tilePoint.X / Defaults.BlockSize);
        tile.Row = (int) Math.Round(tilePoint.Y / Defaults.BlockSize);

        MoveToSpace(offset.X > offset.Y);
        double xmax = 0, ymax = 0;
        for (int i = 0; i < dGrid.Children.Count; i++)
        {
            Tile t = dGrid.Children[i] as Tile;
            double txmax = t.Row * Defaults.BlockSize + t.ActualWidth;
            double tymax = t.Column * Defaults.BlockSize + t.ActualHeight;
            xmax = txmax > xmax ? txmax : xmax;
            ymax = tymax > ymax ? tymax : ymax;
            Panel.SetZIndex(dGrid.Children[i], i);
            Panel.SetZIndex(tile, dGrid.Children.Count);
        }
        (dGrid.Width, dGrid.Height) = (xmax + Defaults.Margin * 2, ymax + Defaults.Margin * 2);
    }

    private void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsDrag || e.LeftButton == MouseButtonState.Released)
            return;
        Tile tile = o as Tile;
        Panel.SetZIndex(tile, int.MaxValue);
        Point mousePoint = e.GetPosition(dGrid);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        if (offset.X != 0 || offset.Y != 0)
            IsDragged = true;
        Canvas.SetLeft(tile, startTilePoint.X + offset.X);
        Canvas.SetTop(tile, startTilePoint.Y + offset.Y);
    }

    public void MoveToSpace(bool moveByRow)
    {
        bool flag = true;
        while (flag)
        {
            flag = false;
            for (int i = 0; i < dGrid.Children.Count; i++)
            {
                if (IntersectsWith(dGrid.Children[i] as Tile))
                {
                    _ = moveByRow ? Row++ : Column++;
                    flag = true;
                    break;
                }
            }
        }
    }

    private bool IntersectsWith(Tile t)
    {
        if (this == t)
            return false;
        Rect r1 = new((double) Canvas.GetLeft(this), (double) Canvas.GetTop(this), ActualWidth, ActualHeight);
        Rect r2 = new((double) Canvas.GetLeft(t), (double) Canvas.GetTop(t), t.ActualWidth, t.ActualHeight);
        return r1.IntersectsWith(r2);
    }
}
