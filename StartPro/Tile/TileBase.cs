using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StartPro;

public partial class TileBase : UserControl
{
    public virtual void Refresh( ) { }

    private static readonly PropertyMetadata TileSizeMeta = new(TileType.Medium, TileSizeChanged);
    private static readonly PropertyMetadata TileColorMeta = new(Defaults.Background, TileColorChanged);
    public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register("TileSize", typeof(TileType), typeof(TileBase), TileSizeMeta);
    public static readonly DependencyProperty TileColorProperty = DependencyProperty.Register("TileColor", typeof(SolidColorBrush), typeof(TileBase), TileColorMeta);

    public Panel Owner => Parent as Panel;

    public TileType TileSize
    {
        get => (TileType) GetValue(TileSizeProperty);
        set => SetValue(TileSizeProperty, value);
    }

    public SolidColorBrush TileColor
    {
        get => (SolidColorBrush) GetValue(TileColorProperty);
        set => SetValue(TileColorProperty, value);
    }
    public int Row
    {
        get => (int) Canvas.GetTop(this) / Defaults.BlockSize;
        set => Canvas.SetTop(this, (value < 0 ? 0 : value) * Defaults.BlockSize);
    }

    public int Column
    {
        get => (int) Canvas.GetLeft(this) / Defaults.BlockSize;
        set => Canvas.SetLeft(this, (value < 0 ? 0 : value) * Defaults.BlockSize);
    }
}
