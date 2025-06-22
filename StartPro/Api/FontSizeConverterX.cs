using System;
using System.Globalization;
using System.Windows.Data;

namespace StartPro.Api;

public class FontSizeConverterX : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return double.TryParse(value.ToString(), out double result)
                && result > 0
                ? result : Defaults.FontSize;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value.ToString( );
}
