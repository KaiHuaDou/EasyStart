using System;
using System.Collections.Generic;
using System.Windows;
using SingleInstanceCore;
using StartPro.Api;
using StartPro.Tile;

namespace StartPro;

public partial class App : Application, ISingleInstance
{
    public static HashSet<TileBase> Tiles { get; set; }
    public static DataStore<Config> Settings { get; set; }
    public static MainWindow TileWindow => Current.MainWindow as MainWindow;

    public static class Program
    {
        [STAThread]
        public static void Main( )
        {
            App app = new( );
            app.InitializeComponent( );
            app.Run( );
            SingleInstance.Cleanup( );
        }
    }

    private void AppExit(object o, ExitEventArgs e)
    {
        TileStore.Save(Tiles);
        Settings.Save( );
    }

    private void AppStartup(object o, StartupEventArgs e)
    {
        if (!this.InitializeAsFirstInstance("KaiHuaDou_StartPro"))
            Current.Shutdown( );

        Settings = new("settings.xml");
        Tiles = TileStore.Load( );

        Resources.MergedDictionaries.Add(new ResourceDictionary( )
        {
            Source = new Uri(Settings.Content.UITheme switch
            {
                0 => "pack://application:,,,/PresentationFramework.Aero,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml",
                1 => "pack://application:,,,/PresentationFramework.Aero2,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Aero2.NormalColor.xaml",
                2 => "pack://application:,,,/PresentationFramework.Luna,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml",
                3 => "pack://application:,,,/PresentationFramework.Luna,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Luna.Homestead.xaml",
                4 => "pack://application:,,,/PresentationFramework.Luna,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Luna.Metallic.xaml",
                5 => "pack://application:,,,/PresentationFramework.Luna,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml",
                6 => "pack://application:,,,/PresentationFramework.Classic,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Classic.xaml",
                _ => "pack://application:,,,/PresentationFramework.Aero,Version=9.0.0.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml",
            })
        });

        MainWindow mainWindow = new( );
        Current.MainWindow = mainWindow;
        mainWindow.Show( );

        Launcher launcher = new( );
        launcher.Show( );
    }

    public void OnInstanceInvoked(string[] args) => TileWindow?.Show( );
}
