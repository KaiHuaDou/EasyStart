using System.Windows.Media;

namespace StartPro.Api;

public static class Defaults
{
    public static string AppName => System.Environment.ProcessPath;
    public static double SizeRate => 0.75;
    public static (int, int) SmallSize => (64, 64);
    public static (int, int) MediumSize => (2 * SmallSize.Item1 + Margin, 2 * SmallSize.Item2 + Margin);
    public static (int, int) WideSize => (4 * SmallSize.Item1 + 3 * Margin, 2 * SmallSize.Item2 + 1 * Margin);
    public static (int, int) HighSize => (2 * SmallSize.Item1 + 1 * Margin, 4 * SmallSize.Item2 + 3 * Margin);
    public static (int, int) LargeSize => (4 * SmallSize.Item1 + 3 * Margin, 4 * SmallSize.Item2 + 3 * Margin);
    public static int BlockSize => SmallSize.Item1 + Margin;
    public static int Radius => App.Program.Settings.Content.UIFlat ? 0 : 10;
    public static int Margin => 10;
    public static int ImageMargin => 15;
    public static double FontSize => 22;
    public static SolidColorBrush Background => new(Color.FromArgb(0xFF, 0xFA, 0xFA, 0xFA));
}
