using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StartPro
{
    public partial class Tile
    {
        public static bool IsDrag { get; set; }
        public static bool[,] TilePos = new bool[64, 64];

        public static void SetTilePos(TileGrid pos, TileType type, bool mark = true)
        {
            TileGrid size = Tile.GetSize(type);
            for (int i = 0; i < size.Row; i++)
                for (int j = 0; j < size.Col; j++)
                    TilePos[pos.Row + i, pos.Col + j] = mark;
        }
        public static bool IsPosEmpty(TileGrid pos, TileType type)
        {
            TileGrid size = Tile.GetSize(type);
            for (int i = 0; i < size.Row; i++)
                for (int j = 0; j < size.Col; j++)
                    if (TilePos[pos.Row + i, pos.Col + j])
                        return false;
            return true;
        }

        #region DependencyProperties
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
            PropertyMetadata appIconMeta = new PropertyMetadata(new BitmapImage( ));
            PropertyMetadata appPathMeta = new PropertyMetadata(Default.AppName, AppPathChanged);
            PropertyMetadata TileSizeMeta = new PropertyMetadata(TileType.Medium);
            PropertyMetadata TileColorMeta = new PropertyMetadata(Default.Background);
            PropertyMetadata TileFontSizeMeta = new PropertyMetadata(Default.FontSize);
            AppNameProperty = DependencyProperty.Register("AppName", typeof(string), thisType, appNameMeta);
            AppIconProperty = DependencyProperty.Register("AppIcon", typeof(ImageSource), thisType, appIconMeta);
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
        public ImageSource AppIcon
        {
            get => (ImageSource) GetValue(AppIconProperty);
            set => SetValue(AppIconProperty, value);
        }
        public string AppPath
        {
            get => (string) GetValue(AppPathProperty);
            set => SetValue(AppPathProperty, value);
        }
        #endregion

    }
}
