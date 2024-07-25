using System.Windows.Media;

namespace StartPro.Api;

public static class Defaults
{
    public static string AppName => System.Environment.ProcessPath;
    public static double SizeRate => 0.75;
    public static int ImageMargin => 15;
    public static double FontSize => 22;
    public static SolidColorBrush Background => new(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA));
}
