using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StartPro.Converter;

public class MaskBrushConverter : IMultiValueConverter
{
    public Brush MaskDeepBrush { get; set; }
    public Brush MaskLightBrush { get; set; }

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        bool isMouseOver = values[0] is bool b0 && b0;
        bool isDragOrClick = (values[1] is bool b1 && b1) || (values[2] is bool b2 && b2);

        return isDragOrClick ? MaskDeepBrush : isMouseOver ? MaskLightBrush : Brushes.Transparent;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException( );
}
