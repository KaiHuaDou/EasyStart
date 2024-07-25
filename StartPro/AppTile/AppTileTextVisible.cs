using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StartPro.Tile;

public class AppTileTextVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => (TileSize) value is TileSize.Small or TileSize.Thin or TileSize.Tall
            ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException( );
}
