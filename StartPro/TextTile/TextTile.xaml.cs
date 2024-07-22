using System.Windows.Controls;
using StartPro.Api;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace StartPro.Tile;

public partial class TextTile : TileBase
{
    public TextTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null;
        border.Child = TextField;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;
    }

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
