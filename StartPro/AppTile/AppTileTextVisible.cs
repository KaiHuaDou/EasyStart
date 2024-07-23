using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro.Tile;

public class AppTileTextVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (TileSize) value == TileSize.Small ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
