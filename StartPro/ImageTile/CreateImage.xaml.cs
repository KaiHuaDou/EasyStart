using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;
public partial class CreateImage : Window, IEditor<ImageTile>
{
    public CreateImage( ) : this(null) { }
    public IEditor<ImageTile> Core => this;
    public ImageTile Item { get; set; }
    public ImageTile Original { get; set; }

    public CreateImage(ImageTile t)
    {
        InitializeComponent( );
        Core.Init(t);
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;
        sizeBox.SelectedIndex = (int) Item.TileSize;
        imageBox.Text = Item.ImagePath;
        stretchBox.SelectedIndex = (int) Item.Stretch;
        Core.InsertTile(mainPanel);
    }

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
        if (Utils.TrySelectFile(out string fileName, "img"))
        {
            imageBox.Text = fileName;
            ImageChanged(o, e);
        }
    }

    private void ShadowBoxChecked(object o, RoutedEventArgs e)
        => Item?.Shadow = shadowBox.IsChecked == true;

    private void StretchChanged(object o, SelectionChangedEventArgs e)
        => Item?.Stretch = (Stretch) stretchBox.SelectedIndex;

    private void TaskCancel(object o, RoutedEventArgs e)
        => Core.OnCancel(this);

    private void TaskOk(object o, RoutedEventArgs e)
        => Core.OnOk(this);

    private void TileSizeChanged(object o, SelectionChangedEventArgs e)
        => Core.OnTileSizeChanged(sizeBox);

    private void WindowClosing(object o, CancelEventArgs e)
        => Core.OnWindowClosing(mainPanel);
}
