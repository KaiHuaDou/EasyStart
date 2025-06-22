using System.Windows;
using System.Windows.Controls;

namespace StartPro.Tile;

public partial class TextTile : TileBase
{
    protected static void TextShadowChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        TextTile textTile = (o as TextTile)!;
        textTile.TileTextShadow.Opacity = (!App.Settings.Content.UIFlat && textTile.TextShadow) ? 0.4 : 0;
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        Panel parent = Parent as Panel;
        parent.Children.Remove(this);
        CreateText c = new(this);
        c.ShowDialog( );
        c.Item.IsEnabled = true;
        parent.Children.Add(c.Item);
        c.Item.Refresh( );
    }
}
