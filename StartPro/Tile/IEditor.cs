using System.Windows;
using System.Windows.Controls;

namespace StartPro.Tile;

public interface IEditor<TTile> where TTile : TileBase, new()
{
    TTile Item { get; set; }
    TTile Original { get; set; }

    Window Owner { get => (this as Window)?.Owner; set => (this as Window)?.Owner = value; }
    void ShowDialog( ) => (this as Window)?.ShowDialog( );

    void Init(TTile t)
    {
        if (t is null)
        {
            Item = new TTile { Row = 0, Column = 0 };
        }
        else
        {
            Original = t;
            Item = TileBase.Clone(t);
        }
        Item.IsEnabled = false;
    }

    void InsertTile(DockPanel mainPanel)
    {
        DockPanel.SetDock(Item, Dock.Right);
        mainPanel.Children.Insert(0, Item);
    }

    void OnCancel(Window window)
    {
        Item = Original;
        window.Close( );
    }

    void OnOk(Window window)
    {
        Item?.IsEnabled = true;
        window.Close( );
    }

    void OnTileSizeChanged(ComboBox sizeBox)
    {
        if (Item != null && sizeBox != null)
            Item.TileSize = (TileSize) sizeBox.SelectedIndex;
    }

    void OnWindowClosing(Panel mainPanel)
    {
        Item?.Margin = new Thickness(TileDatas.BaseMargin);
        mainPanel?.Children.Remove(Item);
    }
}
