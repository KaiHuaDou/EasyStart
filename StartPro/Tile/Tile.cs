using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StartPro;

public partial class Tile
{
    public Tile( )
    {
        InitializeComponent( );
        Init( );
    }

    public void Init( )
        => border.DataContext = this;

    private static readonly PropertyMetadata appNameMeta = new("Application");
    private static readonly PropertyMetadata appIconMeta = new(AppIconChanged);
    private static readonly PropertyMetadata appPathMeta = new(Defaults.AppName, AppPathChanged);
    private static readonly PropertyMetadata TileSizeMeta = new(TileType.Medium);
    private static readonly PropertyMetadata TileColorMeta = new(Defaults.Background);
    private static readonly PropertyMetadata ImageShadowMeta = new(true);
    private static readonly PropertyMetadata ShadowMeta = new(true);
    public static readonly DependencyProperty AppNameProperty = DependencyProperty.Register("AppName", typeof(string), typeof(Tile), appNameMeta);
    public static readonly DependencyProperty AppIconProperty = DependencyProperty.Register("AppIcon", typeof(string), typeof(Tile), appIconMeta);
    public static readonly DependencyProperty AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), typeof(Tile), appPathMeta);
    public static readonly DependencyProperty TileSizeProperty = DependencyProperty.Register("TileSize", typeof(TileType), typeof(Tile), TileSizeMeta);
    public static readonly DependencyProperty TileColorProperty = DependencyProperty.Register("TileColor", typeof(SolidColorBrush), typeof(Tile), TileColorMeta);
    public static readonly DependencyProperty ImageShadowProperty = DependencyProperty.Register("TileImageShadow", typeof(bool), typeof(Tile), ImageShadowMeta);
    public static readonly DependencyProperty ShadowProperty = DependencyProperty.Register("TileShadow", typeof(bool), typeof(Tile), ShadowMeta);

    [DefaultValue(TileType.Medium)]
    public TileType TileSize
    {
        get => (TileType) GetValue(TileSizeProperty);
        set
        {
            SetValue(TileSizeProperty, value);
            Height = Convert.ToDouble(new SizeConverter( ).Convert(value, null, "Height", null));
            Width = Convert.ToDouble(new SizeConverter( ).Convert(value, null, "Width", null));
            Margin = new Thickness(Defaults.Margin);
            border.CornerRadius = (CornerRadius) new RadiusConverter( ).Convert(value, null, null, null);
        }
    }

    public SolidColorBrush TileColor
    {
        get => (SolidColorBrush) GetValue(TileColorProperty);
        set => SetValue(TileColorProperty, value);
    }

    public string AppName
    {
        get => (string) GetValue(AppNameProperty);
        set => SetValue(AppNameProperty, value);
    }

    public string AppIcon
    {
        get => (string) GetValue(AppIconProperty);
        set => SetValue(AppIconProperty, value);
    }

    public string AppPath
    {
        get => (string) GetValue(AppPathProperty);
        set => SetValue(AppPathProperty, value);
    }

    public bool ImageShadow
    {
        get => (bool) GetValue(ImageShadowProperty);
        set
        {
            SetValue(ImageShadowProperty, value);
            TileImageShadow.Opacity = value ? 0.4 : 0;
        }
    }

    public bool Shadow
    {
        get => (bool) GetValue(ShadowProperty);
        set
        {
            SetValue(ShadowProperty, value);
            TileShadow.Opacity = value ? 0.4 : 0;
        }
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
