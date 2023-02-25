using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StartPro
{
    public partial class Tile
    {
        public void Init( )
        {
            SetTilePos(Pos, TileSize, true);
            TileGrid grid = GetSize( );
            Grid.SetRowSpan(this, grid.Row);
            Grid.SetColumnSpan(this, grid.Col);
            MouseRightButtonDown += TileDragStart;
            MouseMove += TileDragging;
            MouseRightButtonUp += TileDragStop;
        }

        public static readonly DependencyProperty AppNameProperty;
        public static readonly DependencyProperty AppIconProperty;
        public static readonly DependencyProperty AppPathProperty;
        public static readonly DependencyProperty TileSizeProperty;
        public static readonly DependencyProperty TileColorProperty;
        public static readonly DependencyProperty TileFontSizeProperty;

        static Tile( )
        {
            Type thisType = typeof(Tile);
            PropertyMetadata appNameMeta = new PropertyMetadata("Application");
            PropertyMetadata appIconMeta = new PropertyMetadata(AppIconChanged);
            PropertyMetadata appPathMeta = new PropertyMetadata(Default.AppName, AppPathChanged);
            PropertyMetadata TileSizeMeta = new PropertyMetadata(TileType.Medium);
            PropertyMetadata TileColorMeta = new PropertyMetadata(Default.Background);
            PropertyMetadata TileFontSizeMeta = new PropertyMetadata(Default.FontSize);
            AppNameProperty = DependencyProperty.Register("AppName", typeof(string), thisType, appNameMeta);
            AppIconProperty = DependencyProperty.Register("AppIcon", typeof(string), thisType, appIconMeta);
            AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), thisType, appPathMeta);
            TileSizeProperty = DependencyProperty.Register("TileSize", typeof(TileType), thisType, TileSizeMeta);
            TileColorProperty = DependencyProperty.Register("TileColor", typeof(SolidColorBrush), thisType, TileColorMeta);
            TileFontSizeProperty = DependencyProperty.Register("TileFontSize", typeof(double), thisType, TileFontSizeMeta);
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
        public double TileFontSize
        {
            get => (double) GetValue(TileFontSizeProperty);
            set => SetValue(TileFontSizeProperty, value);
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
    }
}
