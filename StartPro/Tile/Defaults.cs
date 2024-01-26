using System.Diagnostics;
using System.Windows.Media;

namespace StartPro;

public static class Defaults
{
    public static string AppName => Process.GetCurrentProcess( ).MainModule.FileName;
    public static int Zoom => 2;
    public static int SmallSize => 64;
    public static int MediumSize => SmallSize * Zoom + Margin;
    public static int WideSize => MediumSize * Zoom;
    public static int LargeSize => MediumSize * Zoom;
    public static int Radius => 10;
    public static int Margin => 10;
    public static int ImageMargin => 15;
    public static double FontSize => 22;
    public static SolidColorBrush Background => new(Color.FromArgb(255, 0xEF, 0xEF, 0xEF));
    public static SolidColorBrush Foreground => (SolidColorBrush) System.Windows.Application.Current.Resources["Foreground"];
    public static Color ColorAdj => Color.FromArgb(0, 30, 30, 30);

}
