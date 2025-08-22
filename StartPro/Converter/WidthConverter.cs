using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro.Converter;

public class WidthConverter : IValueConverter
{
    private const double COUNT = 8.0;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => SystemParameters.WorkArea.Width / COUNT - 10.0 - 23.0 / COUNT;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException( );
}
