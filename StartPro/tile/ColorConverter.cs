using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StartPro
{
    internal class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => new SolidColorBrush(((SolidColorBrush) value).Color - Default.ColorAdj);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException( );
    }
}
