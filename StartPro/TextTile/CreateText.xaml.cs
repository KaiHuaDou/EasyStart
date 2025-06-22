using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            Original = FastCopy.Copy(Item);
        }

        Item.IsEnabled = false;
        Title = t is null ? StartPro.Resources.Tile.TitleCreate : StartPro.Resources.Tile.TitleEdit;

        Item.SetBinding(TextTile.TileSizeProperty,
            new Binding( )
            {
                Source = sizeBox,
                Path = new PropertyPath("SelectedIndex"),
                Converter = new EnumIntConverter( ),
            }
        );
        Item.SetBinding(TextTile.FontSizeProperty,
            new Binding( )
            {
                Source = FontSizeBox,
                Path = new PropertyPath("IsChecked"),
                Converter = new FontSizeConverterX( ),
            }
        );
        Item.SetBinding(TextTile.ShadowProperty,
            new Binding( )
            {
                Source = ShadowBox,
                Path = new PropertyPath("IsChecked"),
                Converter = new BoolConverter( ),
            }
        );
        Item.SetBinding(TextTile.TextShadowProperty,
            new Binding( )
            {
                Source = TextShadowBox,
                Path = new PropertyPath("IsChecked"),
                Converter = new BoolConverter( ),
            }
        );
        Item.SetBinding(TextTile.TextProperty,
            new Binding( )
            {
                Source = ContentBox,
                Path = new PropertyPath("Text")
            }
        );
        Item.SetBinding(TextTile.VerticalAlignmentProperty,
            new Binding( )
            {
                Source = VerticalAlignmentBox,
                Path = new PropertyPath("SelectedIndex"),
                Converter = new AlignmentEnumIntConverter( ),
            }
        );
        Item.SetBinding(TextTile.HorizontalAlignmentProperty,
            new Binding( )
            {
                Source = HorizontalAlignmentBox,
                Path = new PropertyPath("SelectedIndex"),
                Converter = new AlignmentEnumIntConverter( ),
            }
        );
        DockPanel.SetDock(Item, Dock.Right);
        mainPanel.Children.Insert(0, Item);
    }

    private void SelectColor(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectColor(out Color color, this))
            Item.TileColor = new SolidColorBrush(color);
    }

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
        Item?.Margin = new Thickness(TileDatas.BaseMargin);
        mainPanel.Children.Remove(Item);
    }
}
