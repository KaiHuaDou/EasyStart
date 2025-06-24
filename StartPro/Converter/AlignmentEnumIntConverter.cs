using System;
using System.Globalization;
using System.Windows.Data;

namespace StartPro.Converter;

public class AlignmentEnumIntConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int) value switch
        {
            0 => 1,
            1 => 0,
            2 => 2,
            3 => 3,
            _ => 1,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.ToObject(targetType, (int) value switch
        {
            1 => 0,
            0 => 1,
            2 => 2,
            3 => 3,
            _ => 0,
        });
    }
}
