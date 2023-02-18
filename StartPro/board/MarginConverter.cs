using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro
{
    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BoardType clipType = (BoardType) value;
            switch (clipType)
            {
                case BoardType.Small:
                    return new Thickness(Default.ImageMargin / Default.Zoom);
                case BoardType.Medium:
                case BoardType.Wide:
                    return new Thickness(
                        Default.ImageMargin,
                        Default.ImageMargin,
                        Default.ImageMargin,
                        Default.ImageMargin / Default.Zoom);
                case BoardType.Large:
                    return new Thickness(
                        Default.ImageMargin * Default.Zoom,
                        Default.ImageMargin * Default.Zoom,
                        Default.ImageMargin * Default.Zoom,
                        Default.ImageMargin / Default.Zoom);
            }
            return Default.ImageMargin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException( );
        }
    }
}
