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
            ClipType clipType = (ClipType) value;
            switch (clipType)
            {
                case ClipType.Small:
                    return new Thickness(Default.ImageMargin / 2.0);
                case ClipType.Medium:
                case ClipType.Wide:
                    return new Thickness(
                        Default.ImageMargin,
                        Default.ImageMargin,
                        Default.ImageMargin,
                        Default.ImageMargin / 2);
                case ClipType.Large:
                    return new Thickness(
                        Default.ImageMargin * 2,
                        Default.ImageMargin * 2,
                        Default.ImageMargin * 2,
                        Default.ImageMargin / 2);
            }
            return Default.ImageMargin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException( );
        }
    }
}
