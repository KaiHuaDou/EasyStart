using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro
{
    public class RadiusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            CornerRadius cValue = (CornerRadius) values[0];
            ClipType cParameter = (ClipType) values[1];
            switch (cParameter)
            {
                case ClipType.Small:
                    return new CornerRadius(cValue.TopLeft / 2.0, cValue.TopRight / 2.0, cValue.BottomRight / 2.0, cValue.BottomLeft / 2.0);
                case ClipType.Medium:
                case ClipType.Wide:
                    return cValue;
                case ClipType.Large:
                    return new CornerRadius(cValue.TopLeft * 2.0, cValue.TopRight * 2.0, cValue.BottomRight * 2.0, cValue.BottomLeft * 2.0);
            }
            return cValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException( );
        }
    }
}
