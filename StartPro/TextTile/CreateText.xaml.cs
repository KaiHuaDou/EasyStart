using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class CreateText : Window
{
    public TextTile Item { get; set; } = new( );
    public TextTile Original { get; set; }

    public CreateText(TextTile t = null)
    {
        InitializeComponent( );

        if (t is null)
        {
            Item = new TextTile { Row = 0, Column = 0 };
        }
        else
        {
            Item = t;
            Original = FastCopy<TextTile>.Copy(Item);
        }

        Item.IsEnabled = false;
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;

        sizeBox.SelectedIndex = (int) Item.TileSize;
        contentBox.Text = Item.Text;
        fontSizeBox.Text = Item.FontSize.ToString( );
        ShadowBox.IsChecked = Item.Shadow;
        TextShadowBox.IsChecked = Item.TextShadow;

        DockPanel.SetDock(Item, Dock.Right);
        mainPanel.Children.Insert(0, Item);
    }
    private void TileSizeChanged(object o, SelectionChangedEventArgs e)
        => Item.TileSize = (TileSize) sizeBox.SelectedIndex;

    private void ContentChanged(object o, RoutedEventArgs e)
        => Item.Text = contentBox.Text;

    private void FontChanged(object o, TextChangedEventArgs e)
    {
        Item.FontSize = double.TryParse(fontSizeBox.Text, out double result)
            ? (result > 0 ? result : Defaults.FontSize)
            : Defaults.FontSize;
    }

    private void SelectColor(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectColor(out Color color))
            Item.TileColor = new SolidColorBrush(color);
    }

    private void ShadowBoxChecked(object o, RoutedEventArgs e)
        => Item.Shadow = (bool) ShadowBox.IsChecked;

    private void TextShadowBoxChecked(object o, RoutedEventArgs e)
        => Item.TextShadow = (bool) TextShadowBox.IsChecked;

    private void TaskCancel(object o, RoutedEventArgs e)
    {
        Item = Original;
        Close( );
    }

    private void TaskOk(object o, RoutedEventArgs e)
    {
        Item.IsEnabled = true;
        Close( );
    }

    private void WindowClosing(object o, CancelEventArgs e)
    {
        if (Item is not null)
            Item.Margin = new Thickness(TileDatas.BaseMargin);
        mainPanel.Children.Remove(Item);
    }

    private void VerticalAlignmentChanged(object o, SelectionChangedEventArgs e)
    {
        Item.TextVerticalAlignment = VerticalAlignmentBox.SelectedIndex switch
        {
            0 => VerticalAlignment.Center,
            1 => VerticalAlignment.Top,
            2 => VerticalAlignment.Bottom,
            _ => VerticalAlignment.Center
        };
    }

    private void HorizontalAlignmentChanged(object o, SelectionChangedEventArgs e)
    {
        Item.TextHorizontalAlignment = HorizontalAlignmentBox.SelectedIndex switch
        {
            0 => HorizontalAlignment.Center,
            1 => HorizontalAlignment.Left,
            2 => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Center
        };
    }
}
