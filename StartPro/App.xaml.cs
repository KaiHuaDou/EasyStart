using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace StartPro;

public partial class App : Application
{
    public static HashSet<Tile> Tiles { get; set; }

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

            Tiles = TileConfig.Load( );
            app.Run( );
        }
    }

    private void AppExit(object o, ExitEventArgs e)
    {
        foreach (Tile tile in Tiles)
            TileConfig.Add(tile);
        TileConfig.Save( );
    }
}
