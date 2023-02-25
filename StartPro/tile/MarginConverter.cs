using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro
{
    internal class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TileType clipType = (TileType) value;
            switch (clipType)
            {
                case TileType.Small: return new Thickness(Default.ImageMargin / Default.Zoom);
                case TileType.Medium:
                case TileType.Wide:
                    return new Thickness(
                        Default.ImageMargin,
                        Default.ImageMargin,
                        Default.ImageMargin,
                        Default.ImageMargin / Default.Zoom);
                case TileType.Large:
                    return new Thickness(
                        Default.ImageMargin * Default.Zoom,
                        Default.ImageMargin * Default.Zoom,
                        Default.ImageMargin * Default.Zoom,
                        Default.ImageMargin / Default.Zoom);
            }
            return Default.ImageMargin;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException( );
    }
}
