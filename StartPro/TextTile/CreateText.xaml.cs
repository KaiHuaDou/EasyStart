using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class CreateText : Window, IEditor<TextTile>
{
    public CreateText( ) : this(null) { }

    public IEditor<TextTile> Core => this;

    public TextTile Item { get; set; }

    public TextTile Original { get; set; }

    public CreateText(TextTile t = null)
    {
        InitializeComponent( );
        Core.Init(t);
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;
        sizeBox.SelectedIndex = (int) Item.TileSize;
        ContentBox.Text = Item.Text;
        VerticalAlignmentBox.SelectedIndex = (int) Item.TextVerticalAlignment;
        HorizontalAlignmentBox.SelectedIndex = (int) Item.TextHorizontalAlignment;
        colorPicker.SelectedColor = (Item.TileColor as SolidColorBrush).Color;
        shadowBox.IsChecked = Item.Shadow;
        Core.InsertTile(mainPanel);
    }

    private void ColorChanged(object o, RoutedEventArgs e)
        => Item?.TileColor = new SolidColorBrush(colorPicker.SelectedColor);

    private void ConfigureText(object o, RoutedEventArgs e)
    {
        TextConfigureDialog dialog = new(Item.TextConfig) { Owner = this };
        dialog.ShowDialog( );
        if (dialog.IsSelected)
        {
            Item.TextConfig = dialog.TextConfig;
        }
    }

    private void ContentBoxTextChanged(object o, TextChangedEventArgs e)
        => Item?.Text = ContentBox.Text;

    private void HorizontalAlignmentBoxSelectionChanged(object o, SelectionChangedEventArgs e)
        => Item?.TextHorizontalAlignment = (HorizontalAlignment) HorizontalAlignmentBox.SelectedIndex;

    private void ShadowBoxChecked(object o, RoutedEventArgs e)
        => Item?.Shadow = shadowBox.IsChecked == true;

    private void TaskCancel(object o, RoutedEventArgs e)
        => Core.OnCancel(this);

    private void TaskOk(object o, RoutedEventArgs e)
        => Core.OnOk(this);

    private void TileSizeChanged(object o, SelectionChangedEventArgs e)
        => Core.OnTileSizeChanged(sizeBox);

    private void VerticalAlignmentBoxSelectionChanged(object o, SelectionChangedEventArgs e)
        => Item?.TextVerticalAlignment = (VerticalAlignment) VerticalAlignmentBox.SelectedIndex;

    private void WindowClosing(object o, CancelEventArgs e)
        => Core.OnWindowClosing(mainPanel);
}
