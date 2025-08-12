using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class CreateApp : Window, IEditor<AppTile>
{
    public AppTile? Item { get; set; }
    public AppTile Original { get; set; }
    public IEditor<AppTile> Core => this;

    public CreateApp( ) : this(null) { }

    public CreateApp(AppTile t)
    {
        InitializeComponent( );
        Core.Init(t);
        OkButton.IsEnabled = t is not null;
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;
        sizeBox.SelectedIndex = (int) Item.TileSize;
        iconBox.Text = Item.AppIcon;
        nameBox.Text = Item.AppName;
        pathBox.Text = Item.AppPath;
        fontBox.Text = Item.FontSize.ToString( );
        shadowBox.IsChecked = Item.Shadow;
        imageShadowBox.IsChecked = Item.ImageShadow;
        colorPicker.SelectedColor = Item.TileColor.Color;
        Core.InsertTile(mainPanel);
    }

    private void ColorChanged(object o, RoutedEventArgs e)
        => Item?.TileColor = new SolidColorBrush(colorPicker.SelectedColor);

    private void FontChanged(object o, TextChangedEventArgs e)
    {
        Item?.FontSize = double.TryParse(fontBox.Text, out double result)
            && result is >= 0.1 and <= 256
            ? result : Defaults.FontSize;
    }

    private void IconChanged(object o, TextChangedEventArgs e)
    {
        try { Item?.AppIcon = new Uri(iconBox.Text).LocalPath; } catch { }
    }

    private void ImageShadowBoxChecked(object o, RoutedEventArgs e)
        => Item?.ImageShadow = (bool) imageShadowBox.IsChecked;

    private void NameChanged(object o, TextChangedEventArgs e)
        => Item?.AppName = nameBox.Text;

    private void PathChanged(object o, RoutedEventArgs e)
    {
        try
        {
            Item?.AppPath = pathBox.Text;
            nameBox.Text = Item?.AppName;
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
