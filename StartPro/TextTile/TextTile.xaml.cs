using System.Windows;
using System.Windows.Controls;
using StartPro.Api;

namespace StartPro.Tile;

public partial class TextTile : TileBase, IEditable<TextTile>
{
    public TextTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null; // 没有任何原因，非要有这一行才能工作。
        userControl = null;
        border.Child = MainText;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;
    }

    public IEditor<TextTile> Editor => new CreateText(this);

    protected static void TextConfigChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        TextTile textTile = (o as TextTile)!;
        textTile.TileTextShadow.Opacity = (!App.Settings.UIFlat && textTile.TextConfig.TextShadow) ? 0.4 : 0;
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        (this as IEditable<TextTile>).Edit(Owner);
    }
}
