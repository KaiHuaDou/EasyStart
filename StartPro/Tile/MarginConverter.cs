using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro;

internal sealed class MarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TileType tileType = (TileType) value;
        return tileType switch
        {
            TileType.Small => new Thickness(Defaults.ImageMargin / 2),
            TileType.Medium or TileType.Wide or TileType.High => new Thickness(
                Defaults.ImageMargin, Defaults.ImageMargin,
                Defaults.ImageMargin, Defaults.ImageMargin / 2),
            TileType.Large => new Thickness(
                Defaults.ImageMargin * 2, Defaults.ImageMargin * 2,
                Defaults.ImageMargin * 2, Defaults.ImageMargin / 2),
            _ => Defaults.ImageMargin,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
