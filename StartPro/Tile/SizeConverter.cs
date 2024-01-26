using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro;

internal sealed class SizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string mode = (string) parameter;
        TileType clipType = (TileType) value;
        if (mode == "Label")
            return clipType == TileType.Small ? Visibility.Collapsed : Visibility.Visible;
        switch (clipType)
        {
            case TileType.Small: return Defaults.SmallSize;
            case TileType.Medium: return Defaults.MediumSize;
            case TileType.Wide:
                if (mode == "Height") return Defaults.WideSize / Defaults.Zoom;
                else if (mode == "Width") return Defaults.WideSize;
                break;
            case TileType.Large: return Defaults.LargeSize;
        }
        return Defaults.MediumSize;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
