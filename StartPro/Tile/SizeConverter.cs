using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro;

internal sealed class SizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        TileType clipType = (TileType) value;
        string mode = (string) parameter;
        if (mode == "Label")
            return clipType == TileType.Small ? Visibility.Collapsed : Visibility.Visible;
        (int, int) size = clipType switch
        {
            TileType.Small => Defaults.SmallSize,
            TileType.Medium => Defaults.MediumSize,
            TileType.Wide => Defaults.WideSize,
            TileType.Large => Defaults.LargeSize,
            _ => Defaults.MediumSize,
        };
        return mode == "Width" ? size.Item2 : size.Item1;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
