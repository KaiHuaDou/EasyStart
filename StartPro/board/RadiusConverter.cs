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
            switch ((BoardType) value)
            {
                case BoardType.Small: return new CornerRadius(Default.Radius / Default.Zoom);
                case BoardType.Medium:
                case BoardType.Wide: return new CornerRadius(Default.Radius);
                case BoardType.Large: return new CornerRadius(Default.Radius * Default.Zoom);
            }
            return new CornerRadius(Default.Radius);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException( );
        }
    }
}
