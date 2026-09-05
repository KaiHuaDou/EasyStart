using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace StartPro.Converter;

public class MaskBrushConverter : IMultiValueConverter
{
    public Brush MaskDeepBrush { get; set; } = Brushes.Transparent;
    public Brush MaskLightBrush { get; set; } = Brushes.Transparent;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var isMouseOver = values[0] is bool b0 && b0;
        var isDragOrClick = (values[1] is bool b1 && b1) || (values[2] is bool b2 && b2);

        return isDragOrClick ? MaskDeepBrush : isMouseOver ? MaskLightBrush : Brushes.Transparent;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException( );
    }
}
