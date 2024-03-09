using System;
using System.Collections.Generic;
using System.Windows;
using SingleInstanceCore;
using StartPro.Api;

namespace StartPro;

public partial class App : Application, ISingleInstance
{
    public static HashSet<Tile> Tiles { get; } = TileConfig.Load( );
    public static DataStore<Config> Setting { get; set; }

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

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
    }

    private void AppExit(object o, ExitEventArgs e)
    {
        foreach (Tile tile in Tiles)
            TileConfig.Add(tile);
        TileConfig.Save( );
        Setting.Save( );
    }

    private void AppStartup(object o, StartupEventArgs e)
    {
        if (!this.InitializeAsFirstInstance("KaiHuaDou_StartPro"))
            Current.Shutdown( );

        Setting = new("settings.xml");
        Resources.MergedDictionaries.Add(new ResourceDictionary( )
        {
            Source = new Uri(Setting.Content.UITheme switch
            {
                0 => "pack://application:,,,/PresentationFramework.Aero,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml",
                1 => "pack://application:,,,/PresentationFramework.Aero2,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Aero2.NormalColor.xaml",
                2 => "pack://application:,,,/PresentationFramework.Luna,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Luna.NormalColor.xaml",
                3 => "pack://application:,,,/PresentationFramework.Luna,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Luna.Homestead.xaml",
                4 => "pack://application:,,,/PresentationFramework.Luna,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Luna.Metallic.xaml",
                5 => "pack://application:,,,/PresentationFramework.Luna,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Royale.NormalColor.xaml",
                6 => "pack://application:,,,/PresentationFramework.Classic,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Classic.xaml",
                _ => "pack://application:,,,/PresentationFramework.Aero,Version=6.0.2.0,Culture=neutral,PublicKeyToken=31bf3856ad364e35;component/themes/Aero.NormalColor.xaml",
            })
        });

        MainWindow mainWindow = new( );
        mainWindow.SwitchState(true);
    }

    public void OnInstanceInvoked(string[] args) { }
}
