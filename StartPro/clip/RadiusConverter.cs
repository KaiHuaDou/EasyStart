using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro
{
    public class RadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ClipType) value)
            {
                case ClipType.Small: return new CornerRadius(Default.Radius / 2.0);
                case ClipType.Medium:
                case ClipType.Wide: return new CornerRadius(Default.Radius);
                case ClipType.Large: return new CornerRadius(Default.Radius * 2.0);
            }
            return new CornerRadius(Default.Radius);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException( );
        }
    }
}
