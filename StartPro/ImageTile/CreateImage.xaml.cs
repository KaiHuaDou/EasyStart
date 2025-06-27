using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using StartPro.Api;

namespace StartPro.Tile;
public partial class CreateImage : Window, IEditor<ImageTile>
{
    public ImageTile? Item { get; set; }
    public ImageTile Original { get; set; }
    public IEditor<ImageTile> Core => this;

    public CreateImage( ) : this(null) { }

    public CreateImage(ImageTile t)
    {
        InitializeComponent( );
        Core.Init(t);
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;
        sizeBox.SelectedIndex = (int) Item.TileSize;
        imageBox.Text = Item.ImagePath;
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
        if (Utils.TrySelectFile(out string fileName, ".png"))
        {
            imageBox.Text = fileName;
            ImageChanged(o, e);
        }
    }

    private void ShadowBoxChecked(object sender, RoutedEventArgs e)
        => Item?.Shadow = ShadowBox.IsChecked == true;

    private void TaskCancel(object o, RoutedEventArgs e)
        => Core.OnCancel(this);

    private void TaskOk(object o, RoutedEventArgs e)
        => Core.OnOk(this);

    private void TileSizeChanged(object sender, SelectionChangedEventArgs e)
        => Core.OnTileSizeChanged(sizeBox);

    private void WindowClosing(object o, CancelEventArgs e)
        => Core.OnWindowClosing(mainPanel);
}
