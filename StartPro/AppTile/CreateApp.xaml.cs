using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class CreateApp : Window, IEditor<AppTile>
{
    public IEditor<AppTile> Core => this;

    public AppTile Item { get; set; }

    public AppTile Original { get; set; }

    public CreateApp( ) : this(null) { }

    public CreateApp(AppTile t)
    {
        InitializeComponent( );
        Core.Init(t);
        OkButton.IsEnabled = t is not null;
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;

        argumentsBox.Text = Item.Arguments;
        colorPicker.SelectedColor = (Item.TileColor as SolidColorBrush).Color;
        fontBox.Text = Item.FontSize.ToString( );
        iconBox.Text = Item.AppIcon;
        imageShadowBox.IsChecked = Item.ImageShadow;
        nameBox.Text = Item.AppName;
        pathBox.Text = Item.AppPath;
        shadowBox.IsChecked = Item.Shadow;
        sizeBox.SelectedIndex = (int) Item.TileSize;

        Core.InsertTile(mainPanel);
    }

    private void ArgumentsChanged(object o, RoutedEventArgs e)
        => Item?.Arguments = argumentsBox.Text;

    private void ColorChanged(object o, RoutedEventArgs e)
        => Item?.TileColor = new SolidColorBrush(colorPicker.SelectedColor);

    private void FontChanged(object o, TextChangedEventArgs e)
    {
        Item?.FontSize = double.TryParse(fontBox.Text, out double result)
            && result is >= 0.1 and <= 256
            ? result : Defaults.FontSize;
    }

    private void IconChanged(object o, TextChangedEventArgs e)
        => Item?.AppIcon = iconBox.Text;

    private void ImageShadowBoxChecked(object o, RoutedEventArgs e)
        => Item?.ImageShadow = (bool) imageShadowBox.IsChecked;

    private void NameChanged(object o, TextChangedEventArgs e)
        => Item?.AppName = nameBox.Text;

    private void PathChanged(object o, RoutedEventArgs e)
    {
        try
        {
            Item?.AppPath = pathBox.Text;
            string name = Path.GetFileNameWithoutExtension(Item?.AppPath);
            nameBox.Text = Item?.AppName = name;
            iconBox.Text = Item?.AppIcon;
            OkButton.IsEnabled = true;
        }
        catch { OkButton.IsEnabled = false; }
    }

    private void SelectExe(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectFile(out string fileName, "exe"))
        {
            pathBox.Text = fileName;
            PathChanged(o, e);
        }
    }

    private void SelectIcon(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectFile(out string fileName, "exe+img"))
            iconBox.Text = fileName;
    }

    private void ShadowBoxChecked(object o, RoutedEventArgs e)
        => Item?.Shadow = shadowBox.IsChecked == true;

    private void TaskCancel(object o, RoutedEventArgs e)
        => Core.OnCancel(this);

    private void TaskOk(object o, RoutedEventArgs e)
        => Core.OnOk(this);

    private void TileSizeChanged(object o, SelectionChangedEventArgs e)
        => Core.OnTileSizeChanged(sizeBox);

    private void WindowClosing(object o, CancelEventArgs e)
        => Core.OnWindowClosing(mainPanel);
}
