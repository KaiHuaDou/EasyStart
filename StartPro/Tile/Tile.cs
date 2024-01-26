using System;
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
    {
        border.DataContext = this;
        SetTilePos(new TileGrid { Col = Column, Row = Row }, TileSize, true);
        TileGrid grid = GetSize( );
        Grid.SetRowSpan(this, grid.Row);
        Grid.SetColumnSpan(this, grid.Col);
    }

    public static readonly DependencyProperty AppNameProperty;
    public static readonly DependencyProperty AppIconProperty;
    public static readonly DependencyProperty AppPathProperty;
    public static readonly DependencyProperty TileSizeProperty;
    public static readonly DependencyProperty TileColorProperty;

    static Tile( )
    {
        Type thisType = typeof(Tile);
        PropertyMetadata appNameMeta = new("Application");
        PropertyMetadata appIconMeta = new(AppIconChanged);
        PropertyMetadata appPathMeta = new(Defaults.AppName, AppPathChanged);
        PropertyMetadata TileSizeMeta = new(TileType.Medium);
        PropertyMetadata TileColorMeta = new(Defaults.Background);
        AppNameProperty = DependencyProperty.Register("AppName", typeof(string), thisType, appNameMeta);
        AppIconProperty = DependencyProperty.Register("AppIcon", typeof(string), thisType, appIconMeta);
        AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), thisType, appPathMeta);
        TileSizeProperty = DependencyProperty.Register("TileSize", typeof(TileType), thisType, TileSizeMeta);
        TileColorProperty = DependencyProperty.Register("TileColor", typeof(SolidColorBrush), thisType, TileColorMeta);
    }

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
    public int Row
    {
        get => Grid.GetRow(this);
        set => Grid.SetRow(this, value);
    }
    public int Column
    {
        get => Grid.GetColumn(this);
        set => Grid.SetColumn(this, value);
    }
}
