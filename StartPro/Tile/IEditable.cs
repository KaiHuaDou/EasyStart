using System.Windows.Controls;

namespace StartPro.Tile;

public interface IEditable<TTile>
    where TTile : TileBase, new()
{
    IEditor<TTile> Editor { get; }

    void Edit(Panel parent)
    {
        parent.Children.Remove(this as TTile);
        IEditor<TTile> dialog = Editor;
        dialog.ShowDialog( );
        dialog.Item.IsEnabled = true;
        parent.Children.Add(dialog.Item);
        dialog.Item.Refresh( );
    }
}
