using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class TileBase : UserControl
{
    public virtual void Refresh( )
    {
        MinHeight = Height = Convert.ToDouble(new SizeConverter( ).Convert(TileSize, null, "Height", null));
        MinWidth = Width = Convert.ToDouble(new SizeConverter( ).Convert(TileSize, null, "Width", null));
        Margin = new Thickness(Defaults.Margin);
        if (Parent is Canvas && Application.Current.MainWindow is MainWindow window)
        {
            // 重新测量并布局确保 ActualWidth 和 ActualHeight 及时更新，以便移动磁贴至适宜位置
            Measure(new Size(window.Width, window.Height));
            Arrange(new Rect(0, 0, window.DesiredSize.Width, window.DesiredSize.Height));
            if (Owner is not null)
                MoveToSpace(Owner, true);
        }
    }

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
