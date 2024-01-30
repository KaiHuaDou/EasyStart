using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro;

internal sealed class RadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (TileType) value switch
        {
            TileType.Small => new CornerRadius(Defaults.Radius / 2),
            TileType.Medium or TileType.Wide => new CornerRadius(Defaults.Radius),
            TileType.Large => new CornerRadius(Defaults.Radius * 2),
            _ => (object) new CornerRadius(Defaults.Radius),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
