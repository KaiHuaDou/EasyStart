using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using SingleInstanceCore;
using StartPro.Api;
using StartPro.Tile;

namespace StartPro;

public partial class App : Application, ISingleInstance
{
    public static List<TileBase> Tiles { get; private set; }
    public static Settings Settings { get; private set; }
    public static ObservableCollection<string> Infos { get; private set; }
    public static MainWindow TileWindow => Current.MainWindow as MainWindow;

    public static class Program
    {
        [STAThread]
        public static void Main( )
        {
            // CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            App app = new( );
            app.InitializeComponent( );
            app.Run( );
            SingleInstance.Cleanup( );
        }
    }

    public static void AddInfo(string message)
    {
        Infos.Add(message);
        TileWindow?.InfoBox?.SelectedItem = message;
    }

    private void AppExit(object o, ExitEventArgs e)
    {
        TileStore.Save( );
        Settings.Write( );
    }

    private void AppStartup(object o, StartupEventArgs e)
    {
        if (!this.InitializeAsFirstInstance("KaiHuaDou_StartPro"))
            Current.Shutdown( );

        Infos = []; // Must initialize first
        Settings = Settings.Read( );
        Tiles = TileStore.Load( );

        Resources.MergedDictionaries.Add(new ResourceDictionary( )
        {
            Source = new Uri(Settings.UITheme switch
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
        mainWindow.Show( );
        Current.MainWindow = mainWindow;
    }

    public void OnInstanceInvoked(string[] args) => TileWindow?.Show( );

    private void AppDispatcherUnhandledException(object o, DispatcherUnhandledExceptionEventArgs e)
    {
#if !DEBUG
        if (MainWindow?.IsLoaded == true)
        {
            AddInfo($"{e.Exception.Message}");
        }
        else
        {
            MessageBox.Show($"{e.Exception.Message}\n{e.Exception.StackTrace}", "严重错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        e.Handled = true;
#endif
    }
}
