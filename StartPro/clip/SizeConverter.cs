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
            ClipType clipType = (ClipType) value;
            if (mode == "Label")
                return clipType == ClipType.Small ? Visibility.Collapsed : Visibility.Visible;
            switch (clipType)
            {
                case ClipType.Small: return Default.SmallSize;
                case ClipType.Medium: return Default.MediumSize;
                case ClipType.Wide:
                    if (mode == "Height")
                        return Default.WideSize / 2.0;
                    else if (mode == "Width")
                        return Default.WideSize;
                    break;
                case ClipType.Large: return Default.LargeSize;
            }
            return Default.MediumSize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException( );
        }
    }
}
