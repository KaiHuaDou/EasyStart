﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;

namespace StartPro.Tile;

public partial class TileBase : UserControl
{
    public virtual void Refresh( )
    {
        (int, int) tileSize = TileSize switch
        {
            TileSize.Small => Defaults.SmallSize,
            TileSize.Medium => Defaults.MediumSize,
            TileSize.Wide => Defaults.WideSize,
            TileSize.Large => Defaults.LargeSize,
            TileSize.High => Defaults.HighSize,
            _ => Defaults.MediumSize,
        };
        MinWidth = Width = tileSize.Item1;
        MinHeight = Height = tileSize.Item2;
        Margin = new Thickness(Defaults.Margin);
        border.DataContext = this;
        border.CornerRadius = maskBorder.CornerRadius = TileSize switch
        {
            TileSize.Small => new CornerRadius(Defaults.Radius / 2),
            TileSize.Medium or TileSize.Wide or TileSize.High => new CornerRadius(Defaults.Radius),
            TileSize.Large => new CornerRadius(Defaults.Radius * 2),
            _ => new CornerRadius(Defaults.Radius),
        };

        if (Parent is Canvas && Application.Current.MainWindow is MainWindow window)
        {
            // 重新测量并布局确保 ActualWidth 和 ActualHeight 及时更新，以便移动磁贴至适宜位置
            Measure(new Size(window.Width, window.Height));
            Arrange(new Rect(0, 0, window.DesiredSize.Width, window.DesiredSize.Height));
            if (Owner is not null)
                MoveToSpace(Owner);
        }
    }

    private static readonly PropertyMetadata TileSizeMeta = new(TileSize.Medium, TileSizeChanged);
    private static readonly PropertyMetadata TileColorMeta = new(Defaults.Background, TileColorChanged);
    private static readonly PropertyMetadata ShadowMeta = new(true);
    public static readonly DependencyProperty TileSizeProperty
        = DependencyProperty.Register("TileSize", typeof(TileSize), typeof(TileBase), TileSizeMeta);
    public static readonly DependencyProperty TileColorProperty
        = DependencyProperty.Register("TileColor", typeof(SolidColorBrush), typeof(TileBase), TileColorMeta);
    public static readonly DependencyProperty ShadowProperty
        = DependencyProperty.Register("TileShadow", typeof(bool), typeof(AppTile), ShadowMeta);

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
        get => (int) Canvas.GetTop(this) / Defaults.BlockSize;
        set => Canvas.SetTop(this, (value < 0 ? 0 : value) * Defaults.BlockSize);
    }

    public int Column
    {
        get => (int) Canvas.GetLeft(this) / Defaults.BlockSize;
        set => Canvas.SetLeft(this, (value < 0 ? 0 : value) * Defaults.BlockSize);
    }

    public bool Shadow
    {
        get => (bool) GetValue(ShadowProperty);
        set
        {
            SetValue(ShadowProperty, value);
            if (TileShadow is not null)
                TileShadow.Opacity = (!App.Program.Settings.Content.UIFlat && value) ? 0.4 : 0;
        }
    }
}
