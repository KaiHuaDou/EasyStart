using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class CreateApp : Window
{
    public AppTile? Item { get; set; }
    public AppTile Original { get; set; }

    public CreateApp(AppTile t = null)
    {
        InitializeComponent( );
        MaxWidth = Defaults.WidthPercent * SystemParameters.PrimaryScreenWidth;

        if (t is null)
        {
            Item = new AppTile { Row = 0, Column = 0 };
        }
        else
        {
            Original = t;
            Item = TileBase.Clone(t);
        }

        Item.IsEnabled = false;
        OkButton.IsEnabled = t is not null;
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;

        sizeBox.SelectedIndex = (int) Item.TileSize;
        iconBox.Text = Item.AppIcon;
        nameBox.Text = Item.AppName;
        pathBox.Text = Item.AppPath;
        fontBox.Text = Item.FontSize.ToString( );
        ShadowBox.IsChecked = Item.Shadow;
        ImageShadowBox.IsChecked = Item.ImageShadow;
        colorPicker.SelectedColor = Item.TileColor.Color;

        DockPanel.SetDock(Item, Dock.Right);
        mainPanel.Children.Insert(0, Item);
    }


    private void ColorChanged(object sender, RoutedEventArgs e)
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
        => Item?.ImageShadow = (bool) ImageShadowBox.IsChecked;

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
        if (Utils.TrySelectExe(out string fileName))
        {
            pathBox.Text = fileName;
            PathChanged(o, e);
        }
    }

    private void SelectIcon(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectImage(out string fileName))
            iconBox.Text = fileName;
    }

    private void ShadowBoxChecked(object o, RoutedEventArgs e)
        => Item?.Shadow = (bool) ShadowBox.IsChecked;

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

    private void TileSizeChanged(object o, SelectionChangedEventArgs e)
        => Item?.TileSize = (TileSize) sizeBox.SelectedIndex;

    private void WindowClosing(object o, CancelEventArgs e)
    {
        Item?.Margin = new Thickness(TileDatas.BaseMargin);
        mainPanel.Children.Remove(Item);
    }
}
