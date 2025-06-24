using System.Windows;
using System.Windows.Media;

namespace StartPro.Api;

public static class Defaults
{
    public static double HeightPercent => 0.75;
    public static double WidthPercent => 0.75;
    public static double FontSize => 19;
    public static FontFamily FontFamily => new("Segoe UI");
    public static FontWeight FontWeight => FontWeights.Normal;
    public static FontStyle FontStyle => FontStyles.Normal;
    public static FontStretch FontStretch => FontStretches.Normal;
    public static TextAlignment TextAlignment => TextAlignment.Center;
    public static SolidColorBrush Foreground => new(Color.FromArgb(0xFF, 0x06, 0x06, 0x06));
    public static SolidColorBrush Background => new(Color.FromArgb(0x55, 0xFA, 0xFA, 0xFA));
}
