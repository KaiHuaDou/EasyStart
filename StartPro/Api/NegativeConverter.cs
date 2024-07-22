using System;
using System.Globalization;
using System.Windows.Data;

namespace StartPro.Api;

public class NegativeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => -(double) value - 48;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => -(double) value - 48;
}
