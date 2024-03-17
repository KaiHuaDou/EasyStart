using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StartPro.Api;

namespace StartPro.Tile;

public class RadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((string) parameter == "MainWindow")
        {
            if (Defaults.Radius == 0)
                return new CornerRadius(0);
            int MainRadius = Defaults.Radius + Defaults.Margin;
            return new CornerRadius(MainRadius, MainRadius, 0, 0);
        }
        else
        {
            return (TileType) value switch
            {
                TileType.Small => new CornerRadius(Defaults.Radius / 2),
                TileType.Medium or TileType.Wide or TileType.High => new CornerRadius(Defaults.Radius),
                TileType.Large => new CornerRadius(Defaults.Radius * 2),
                _ => new CornerRadius(Defaults.Radius),
            };
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
