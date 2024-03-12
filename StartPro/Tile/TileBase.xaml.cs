using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro;

public partial class TileBase : UserControl
{
    protected static void TileColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {

    }

    protected static void TileSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        TileBase tile = o as TileBase;
        tile.Refresh( );
    }

    protected void TileLeftButtonUp(object o, MouseButtonEventArgs e)
        => TileDragStop(o, e);
}
