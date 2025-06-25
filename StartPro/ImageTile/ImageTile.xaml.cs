using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StartPro.Api;

namespace StartPro.Tile;

public partial class ImageTile : TileBase
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

    protected static void ImagePathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        ImageTile tile = o as ImageTile;
        tile.MainImage.Source = new BitmapImage(new Uri(tile.ImagePath));
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        Panel parent = Parent as Panel;
        parent.Children.Remove(this);
        CreateImage dialog = new(this);
        dialog.ShowDialog( );
        dialog.Item.IsEnabled = true;
        parent.Children.Add(dialog.Item);
        dialog.Item.Refresh( );
    }
}
