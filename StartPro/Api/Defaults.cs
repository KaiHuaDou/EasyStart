using System.Windows;
using System.Windows.Media;

namespace StartPro.Api;

public static class Defaults
{
    // 窗口大小
    public static readonly double HeightPercent = 1;
    public static readonly double WidthPercent = 1;

    // 文本相关
    public static readonly double FontSize = SystemFonts.MenuFontSize;
    public static readonly FontFamily FontFamily = SystemFonts.MenuFontFamily;
    public static readonly FontWeight FontWeight = FontWeights.Normal;
    public static readonly FontStyle FontStyle = FontStyles.Normal;
    public static readonly FontStretch FontStretch = FontStretches.Normal;
    public static readonly TextAlignment TextAlignment = TextAlignment.Center;

    // 颜色相关
    public static readonly string ForegroundColorText = "#FF060606";
    public static readonly Color ForegroundColor = Color.FromArgb(0xFF, 0x06, 0x06, 0x06);
    public static readonly SolidColorBrush Foreground = new(ForegroundColor);
    public static readonly string BackgroundColorText = "#FFFAFAFA";
    public static readonly Color BackgroundColor = Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA);
    public static readonly SolidColorBrush Background = new(BackgroundColor);
    public static readonly Color TileColor = Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF);
    public static readonly SolidColorBrush TileColorBrush = new(TileColor);
}
