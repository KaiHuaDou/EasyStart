using System;
using System.Windows;

namespace StartPro
{
    public partial class App : Application
    {
        public class Program
        {
            [STAThread]
            public static void Main( )
            {
                App app = new App( );
                app.InitializeComponent( );
                app.Run( );
            }
        }
    }
}
