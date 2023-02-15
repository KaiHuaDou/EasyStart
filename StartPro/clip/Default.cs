using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace StartPro
{
    public static class Default
    {
        public static string AppName => Process.GetCurrentProcess( ).MainModule.FileName;
        public static int SmallSize => 64;
        public static int MediumSize => SmallSize * 2;
        public static int WideSize => SmallSize * 4;
        public static int LargeSize => SmallSize * 4;
        public static int Radius => 32;
        public static Thickness Margin => new Thickness(10);
        public static int ImageMargin => 20;
        public static double FontSize => 22;
        public static SolidColorBrush Background => new SolidColorBrush(new Color { A = 255, R = 0, G = 0, B = 128 });

    }
}
