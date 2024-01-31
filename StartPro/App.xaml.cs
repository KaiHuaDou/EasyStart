using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace StartPro;

public partial class App : Application
{
    public static class Program
    {
        [STAThread]
        public static void Main( )
        {
#if DEBUG
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
#endif
            App app = new( );
            app.InitializeComponent( );
            app.Run( );
        }
    }
}
