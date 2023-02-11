using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace StartPro
{
    public static class Setting
    {
        public static string ProcessName
        {
            get { return Process.GetCurrentProcess( ).MainModule.FileName; }
        }
        public static int SmallSize { get { return 64; } }
        public static int MediumSize { get { return SmallSize * 2; } }
        public static int WideSize { get { return SmallSize * 4; } }
        public static int LargeSize { get { return SmallSize * 4; } }
        public static CornerRadius Radius { get { return new CornerRadius(8); } }
        public static double FontSize { get { return 14; } }
        public static SolidColorBrush Background { get { return new SolidColorBrush(new Color { A = 255, R = 0, G = 0, B = 0 }); ; } }

    }
}
