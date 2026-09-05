using System.Windows.Controls;

namespace StartPro.Tile;

public interface IEditable<TTile>
    where TTile : TileBase, new()
{
    IEditor<TTile> Editor { get; }

    void Edit(Panel parent)
    {
        parent.Children.Remove(this as TTile);
        var dialog = Editor;
        dialog.ShowDialog( );
        var item = dialog.Item;
        item?.IsEnabled = true;
        parent.Children.Add(item);
        item?.Refresh( );
    }
}
