using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StartPro.Api;

namespace StartPro.Tile;

public class MarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TileSize tileType = (TileSize) value;
        return tileType switch
        {
            TileSize.Small => new Thickness(Defaults.ImageMargin / 2),
            TileSize.Medium or TileSize.Wide or TileSize.High => new Thickness(
                Defaults.ImageMargin, Defaults.ImageMargin,
                Defaults.ImageMargin, Defaults.ImageMargin / 2),
            TileSize.Large => new Thickness(
                Defaults.ImageMargin * 2, Defaults.ImageMargin * 2,
                Defaults.ImageMargin * 2, Defaults.ImageMargin / 2),
            _ => new Thickness(Defaults.ImageMargin),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
