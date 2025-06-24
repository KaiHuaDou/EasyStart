using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class CreateText : Window
{
    public TextTile? Item { get; set; } = new( );

    public TextTile Original { get; set; }

    public CreateText(TextTile t = null)
    {
        InitializeComponent( );
        MaxWidth = Defaults.WidthPercent * SystemParameters.PrimaryScreenWidth;

        if (t is null)
        {
            Item = new TextTile { Row = 0, Column = 0 };
        }
        else
        {
            Original = t;
            Item = TileBase.Clone(t);
        }

        Item.IsEnabled = false;
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;

        ContentBox.Text = Item.Text;
        VerticalAlignmentBox.SelectedIndex = (int) Item.TextVerticalAlignment;
        HorizontalAlignmentBox.SelectedIndex = (int) Item.TextHorizontalAlignment;
        ShadowBox.IsChecked = Item.Shadow;

        DockPanel.SetDock(Item, Dock.Right);
        mainPanel.Children.Insert(0, Item);
    }

    private void ConfigureText(object o, RoutedEventArgs e)
    {
        TextConfigureDialog dialog = new(Item.TextConfig)
        {
            Owner = this
        };
        dialog.ShowDialog( );
        if (dialog.IsSelected)
        {
            Item.TextConfig = dialog.TextConfig;
        }
    }

    private void ContentBoxTextChanged(object sender, TextChangedEventArgs e)
        => Item?.Text = ContentBox.Text;

    private void HorizontalAlignmentBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        => Item?.TextHorizontalAlignment = (HorizontalAlignment) HorizontalAlignmentBox.SelectedIndex;

    private void SelectColor(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectColor(Item.TileColor.Color, out Color color, this))
            Item.TileColor = new SolidColorBrush(color);
    }
    private void ShadowBoxChecked(object sender, RoutedEventArgs e)
        => Item?.Shadow = ShadowBox.IsChecked == true;

    private void TileSizeChanged(object sender, SelectionChangedEventArgs e)
        => Item?.TileSize = (TileSize) sizeBox.SelectedIndex;

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

    private void VerticalAlignmentBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        => Item?.TextVerticalAlignment = (VerticalAlignment) VerticalAlignmentBox.SelectedIndex;

    private void WindowClosing(object o, CancelEventArgs e)
    {
        Item?.Margin = new Thickness(TileDatas.BaseMargin);
        mainPanel.Children.Remove(Item);
    }
}
