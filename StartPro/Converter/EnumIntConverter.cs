using System;
using System.Globalization;
using System.Windows.Data;

namespace StartPro.Converter;

public class EnumIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (int) value;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Enum.ToObject(targetType, (int) value);
}
