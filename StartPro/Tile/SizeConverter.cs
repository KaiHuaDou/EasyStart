using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro;

public class SizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TileType tileType = (TileType) value;
        string mode = (string) parameter;
        if (mode == "Label")
            return tileType == TileType.Small ? Visibility.Collapsed : Visibility.Visible;
        (int, int) size = tileType switch
        {
            TileType.Small => Defaults.SmallSize,
            TileType.Medium => Defaults.MediumSize,
            TileType.Wide => Defaults.WideSize,
            TileType.Large => Defaults.LargeSize,
            TileType.High => Defaults.HighSize,
            _ => Defaults.MediumSize,
        };
        return mode == "Width" ? size.Item1 : size.Item2;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
