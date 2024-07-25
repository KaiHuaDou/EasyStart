using System.Windows.Controls;

namespace StartPro.Tile;

public partial class TextTile : TileBase
{
    private void EditTile(object o, System.Windows.RoutedEventArgs e)
    {
        Panel parent = Parent as Panel;
        parent.Children.Remove(this);
        CreateText c = new(this);
        c.ShowDialog( );
        c.Item.IsEnabled = true;
        parent.Children.Add(c.Item);
        c.Item.MoveToSpace(parent);
    }
}
