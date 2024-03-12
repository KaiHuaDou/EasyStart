using System;
using System.Collections.Generic;
using System.Windows;
using SingleInstanceCore;
using StartPro.Api;

namespace StartPro;

public partial class App : Application, ISingleInstance
{
    public static HashSet<TileBase> Tiles { get; } = TileConfig.Load( );
    public static MainWindow TileWindow => Current.MainWindow as MainWindow;

    public static class Program
    {
        public static DataStore<Config> Settings { get; set; } = new("settings.xml");

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
        foreach (TileBase tile in Tiles)
            TileConfig.Add(tile);
        TileConfig.Save( );
        Program.Settings.Save( );
    }

    private void AppStartup(object o, StartupEventArgs e)
    {
        if (!this.InitializeAsFirstInstance("KaiHuaDou_StartPro"))
            Current.Shutdown( );

        Resources.MergedDictionaries.Add(new ResourceDictionary( )
        {
            Source = new Uri(Program.Settings.Content.UITheme switch
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
        QuickStart quickStart = new( );
        Current.MainWindow = mainWindow;
        quickStart.Show( );
        mainWindow.Show( );
    }

    public void OnInstanceInvoked(string[] args) => TileWindow?.Show( );
}
