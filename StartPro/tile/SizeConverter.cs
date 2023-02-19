using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro
{
    internal class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string mode = (string) parameter;
            TileType clipType = (TileType) value;
            if (mode == "Label")
                return clipType == TileType.Small ? Visibility.Collapsed : Visibility.Visible;
            switch (clipType)
            {
                case TileType.Small:
                    return Default.SmallSize;
                case TileType.Medium:
                    return Default.MediumSize;
                case TileType.Wide:
                    if (mode == "Height")
                        return Default.WideSize / Default.Zoom;
                    else if (mode == "Width")
                        return Default.WideSize;
                    break;
                case TileType.Large:
                    return Default.LargeSize;
            }
            return Default.MediumSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException( );
        }
    }
}
