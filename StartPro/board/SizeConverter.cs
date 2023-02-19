using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string mode = (string) parameter;
            BoardType clipType = (BoardType) value;
            if (mode == "Label")
                return clipType == BoardType.Small ? Visibility.Collapsed : Visibility.Visible;
            switch (clipType)
            {
                case BoardType.Small:
                    return Default.SmallSize;
                case BoardType.Medium:
                    return Default.MediumSize;
                case BoardType.Wide:
                    if (mode == "Height")
                        return Default.WideSize / Default.Zoom;
                    else if (mode == "Width")
                        return Default.WideSize;
                    break;
                case BoardType.Large:
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
