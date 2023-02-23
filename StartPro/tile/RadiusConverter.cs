using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro
{
    internal class RadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((TileType) value)
            {
                case TileType.Small: return new CornerRadius(Default.Radius / Default.Zoom);
                case TileType.Medium:
                case TileType.Wide: return new CornerRadius(Default.Radius);
                case TileType.Large: return new CornerRadius(Default.Radius * Default.Zoom);
            }
            return new CornerRadius(Default.Radius);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException( );
    }
}
