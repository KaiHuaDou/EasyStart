using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StartPro.Api;

namespace StartPro;

public class RadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (Defaults.Radius == 0)
            return new CornerRadius(0);
        int MainRadius = Defaults.Radius + Defaults.Margin;
        return new CornerRadius(MainRadius, MainRadius, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
