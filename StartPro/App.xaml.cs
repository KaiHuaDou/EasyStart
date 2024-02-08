using System;
using System.Collections.Generic;
using System.Windows;

namespace StartPro;

public partial class App : Application
{
    public static HashSet<Tile> Tiles { get; set; }

    public static class Program
    {
        [STAThread]
        public static void Main( )
        {
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

    private void AppStartup(object o, StartupEventArgs e)
    {
        MainWindow mainWindow = new( );
        mainWindow.SwitchState(true);
    }
}
