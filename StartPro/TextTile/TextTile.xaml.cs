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
        userControl.Content = null;
        border.Child = TextField;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;
    }

    public IEditor<TextTile> Editor => new CreateText(this);

    protected static void TextConfigChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        TextTile textTile = (o as TextTile)!;
        textTile.TileTextShadow.Opacity = (!App.Settings.Content.UIFlat && textTile.TextConfig.TextShadow) ? 0.4 : 0;
    }
    private void EditTile(object o, RoutedEventArgs e)
    {
        (this as IEditable<TextTile>).Edit(Owner);
    }
}
