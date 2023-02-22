using System.Diagnostics;
using System.Windows.Media;

namespace StartPro
{
    public static class Default
    {
        public static string AppName => Process.GetCurrentProcess( ).MainModule.FileName;
        public static int Zoom => 2;
        public static int SmallSize => 64;
        public static int MediumSize => SmallSize * Zoom + Margin;
        public static int WideSize => SmallSize * Zoom * Zoom + Margin * Zoom;
        public static int LargeSize => SmallSize * Zoom * Zoom;
        public static int Radius => 24;
        public static int Margin => 10;
        public static int ImageMargin => 15;
        public static double FontSize => 22;
        public static SolidColorBrush Background => new SolidColorBrush(Color.FromArgb(255, 0x4F,  0x4F,  0x4F ));
        public static Color ColorAdj => Color.FromRgb(60, 60, 60);

    }
}
