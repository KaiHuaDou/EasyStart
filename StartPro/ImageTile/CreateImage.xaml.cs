using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using StartPro.Api;

namespace StartPro.Tile;
public partial class CreateImage : Window
{
    public CreateImage(ImageTile t = null)
    {
        InitializeComponent( );
        MaxWidth = Defaults.WidthPercent * SystemParameters.PrimaryScreenWidth;

        if (t is null)
        {
            Item = new ImageTile { Row = 0, Column = 0 };
        }
        else
        {
            Original = t;
            Item = TileBase.Clone(t);
        }

        Item.IsEnabled = false;
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;

        sizeBox.SelectedIndex = (int) Item.TileSize;
        imageBox.Text = Item.ImagePath;

        DockPanel.SetDock(Item, Dock.Right);
        mainPanel.Children.Insert(0, Item);
    }

    public ImageTile? Item { get; set; } = new( );

    public ImageTile Original { get; set; }
    private void ImageChanged(object o, RoutedEventArgs e)
    {
        try
        {
            Item?.ImagePath = imageBox.Text;
            OkButton.IsEnabled = true;
        }
        catch { OkButton.IsEnabled = false; }
    }

    private void SelectImage(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectFile(out string fileName, ".png"))
        {
            imageBox.Text = fileName;
            ImageChanged(o, e);
        }
    }

    private void ShadowBoxChecked(object sender, RoutedEventArgs e)
        => Item?.Shadow = ShadowBox.IsChecked == true;

    private void TaskCancel(object o, RoutedEventArgs e)
    {
        Item = Original;
        Close( );
    }

    private void TaskOk(object o, RoutedEventArgs e)
    {
        Item?.IsEnabled = true;
        Close( );
    }

    private void TileSizeChanged(object sender, SelectionChangedEventArgs e)
                => Item?.TileSize = (TileSize) sizeBox.SelectedIndex;
    private void WindowClosing(object o, CancelEventArgs e)
    {
        Item?.Margin = new Thickness(TileDatas.BaseMargin);
        mainPanel.Children.Remove(Item);
    }

}
