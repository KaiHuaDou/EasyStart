using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using StartPro.Tile;

namespace StartPro;

public class RadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (TileDatas.BaseRadius == 0)
            return new CornerRadius(0);
        int MainRadius = TileDatas.BaseRadius + TileDatas.BaseMargin;
        return new CornerRadius(MainRadius, MainRadius, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
