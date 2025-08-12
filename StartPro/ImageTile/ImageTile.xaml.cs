using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StartPro.Api;

namespace StartPro.Tile;

public partial class ImageTile : TileBase, IEditable<ImageTile>
{
    public ImageTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null;
        border.Child = MainImage;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;
    }

    public IEditor<ImageTile> Editor => new CreateImage(this);

    protected static void ImagePathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        ImageTile tile = o as ImageTile;
        tile.MainImage.Source = new BitmapImage(new Uri(tile.ImagePath));
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        (this as IEditable<ImageTile>).Edit(Parent as Panel);
    }
}
