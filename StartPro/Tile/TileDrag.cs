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
        IsDrag = false;
        Tile tile = o as Tile;
        tile?.ReleaseMouseCapture( );

        Point mousePoint = e.GetPosition(dGrid);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        Point tilePoint = new(startTilePoint.X + offset.X, startTilePoint.Y + offset.Y);

        tile.Column = (int) tilePoint.X / Defaults.BlockSize;
        tile.Row = (int) tilePoint.Y / Defaults.BlockSize;

        for (int i = 0; i < dGrid.Children.Count; i++)
            Panel.SetZIndex(dGrid.Children[i], i);
        Panel.SetZIndex(tile, dGrid.Children.Count);
    }

    private void TileDragging(object o, MouseEventArgs e)
    {
        if (!IsDrag || e.RightButton == MouseButtonState.Released)
            return;
        Tile tile = o as Tile;
        Panel.SetZIndex(tile, int.MaxValue);
        Point mousePoint = e.GetPosition(dGrid);
        Point offset = new(mousePoint.X - startMousePoint.X, mousePoint.Y - startMousePoint.Y);
        Canvas.SetLeft(tile, startTilePoint.X + offset.X);
        Canvas.SetTop(tile, startTilePoint.Y + offset.Y);
    }
}
