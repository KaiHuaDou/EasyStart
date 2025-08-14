using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class TileBase : UserControl
{
    private static readonly PropertyMetadata TileSizeMeta = new(TileSize.Medium, TileSizeChanged);
    private static readonly PropertyMetadata TileColorMeta = new(Defaults.TileColor, TileColorChanged);
    private static readonly PropertyMetadata ShadowMeta = new(true, TileShadowChanged);
    public static DependencyProperty TileSizeProperty
        = DependencyProperty.Register("TileSize", typeof(TileSize), typeof(TileBase), TileSizeMeta);
    public static readonly DependencyProperty TileColorProperty
        = DependencyProperty.Register("TileColor", typeof(SolidColorBrush), typeof(TileBase), TileColorMeta);
    public static readonly DependencyProperty ShadowProperty
        = DependencyProperty.Register("TileShadow", typeof(bool), typeof(TileBase), ShadowMeta);

    public Panel Owner => Parent as Panel;

    public TileSize TileSize
    {
        get => (TileSize) GetValue(TileSizeProperty);
        set => SetValue(TileSizeProperty, value);
    }

    public SolidColorBrush TileColor
    {
        get => (SolidColorBrush) GetValue(TileColorProperty);
        set => SetValue(TileColorProperty, value);
    }

    public int Row
    {
        get => (int) Canvas.GetTop(this) / TileDatas.BlockSize;
        set => Canvas.SetTop(this, (value < 0 ? 0 : value) * TileDatas.BlockSize);
    }

    public int Column
    {
        get => (int) Canvas.GetLeft(this) / TileDatas.BlockSize;
        set => Canvas.SetLeft(this, (value < 0 ? 0 : value) * TileDatas.BlockSize);
    }

    public bool Shadow
    {
        get => (bool) GetValue(ShadowProperty);
        set => SetValue(ShadowProperty, value);
    }
}
