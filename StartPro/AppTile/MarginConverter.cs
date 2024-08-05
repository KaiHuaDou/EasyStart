using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro.Tile;

public class MarginConverter : IValueConverter
{
    public static int ImageMargin => 15;

    public static Dictionary<TileSize, Thickness> ImageMargins => new( )
    {
        {TileSize.Small  , new Thickness(ImageMargin / 2) },
        {TileSize.Medium , new Thickness(ImageMargin, ImageMargin, ImageMargin, ImageMargin / 2) },
        {TileSize.Thin   , new Thickness(ImageMargin / 2) },
        {TileSize.Wide   , new Thickness(ImageMargin, ImageMargin, ImageMargin, ImageMargin / 2) },
        {TileSize.Tall   , new Thickness(ImageMargin / 2) },
        {TileSize.High   , new Thickness(ImageMargin, ImageMargin, ImageMargin, ImageMargin / 2) },
        {TileSize.Large  , new Thickness(ImageMargin * 2, ImageMargin * 2, ImageMargin * 2, ImageMargin / 2) },
    };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => ImageMargins[(TileSize) value];
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
