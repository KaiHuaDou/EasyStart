using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro;

internal sealed class MarginConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TileType clipType = (TileType) value;
        return clipType switch
        {
            TileType.Small => new Thickness(Defaults.ImageMargin / Defaults.Zoom),
            TileType.Medium or TileType.Wide => new Thickness(
                                    Defaults.ImageMargin,
                                    Defaults.ImageMargin,
                                    Defaults.ImageMargin,
                                    Defaults.ImageMargin / Defaults.Zoom),
            TileType.Large => new Thickness(
                                    Defaults.ImageMargin * Defaults.Zoom,
                                    Defaults.ImageMargin * Defaults.Zoom,
                                    Defaults.ImageMargin * Defaults.Zoom,
                                    Defaults.ImageMargin / Defaults.Zoom),
            _ => Defaults.ImageMargin,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
