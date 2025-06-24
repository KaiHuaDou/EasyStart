using System;
using System.Globalization;
using System.Windows.Data;
using StartPro.Api;

namespace StartPro.Converter;

public class FontSizeConverterX : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => Utils.ToFontSize(value.ToString( ));


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value.ToString( );
}
