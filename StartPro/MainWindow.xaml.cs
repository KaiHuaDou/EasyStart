using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using StartPro.Api;
using StartPro.Tile;
using static StartPro.External.NativeMethods;

namespace StartPro;

public partial class MainWindow : Window
{
    public MainWindow( )
    {
        InitializeComponent( );

        Height = Defaults.HeightPercent * SystemParameters.PrimaryScreenHeight;
        Width = Defaults.WidthPercent * SystemParameters.PrimaryScreenWidth;
        TilePanel.MinHeight = Height - 256;
        TilePanel.MinWidth = Width - 96;
        Top = SystemParameters.WorkArea.Height - Height;
        Left = (SystemParameters.WorkArea.Width - Width) / 2;
        LoadBackground( );

        AppList.ItemsSource = new Collection<StartMenuApp>{
            new( ) {
                AppName = "Loading",
                AppPath = "",
                AppIcon = new BitmapImage()
            }
        };

        foreach (TileBase tile in App.Tiles)
        {
            TilePanel.Children.Add(tile);
            tile.Refresh( );
        }
    }

    public new void Hide( )
    {
        if (Resources["HideWindow"] is Storyboard hideAnimation)
        {
            hideAnimation.Completed += (o, e) => base.Hide( );
            hideAnimation.Begin(MainBorder);
        }
    }

    public new void Show( )
    {
        if (Resources["ShowWindow"] is Storyboard showAnimation)
        {
            base.Show( );
            showAnimation.Begin(MainBorder);
        }
        SetForegroundWindow(handle);
    }

    public void ShowHide( )
    {
        if (Visibility == Visibility.Hidden) Show( );
        else Hide( );
    }

    private void AddAppTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateApp window = new( );
        window.ShowDialog( );
        Show( );
        AppTile tile = window.Item;
        if (tile?.IsEnabled != true)
            return;
        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void AddImageTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateImage window = new( );
        window.ShowDialog( );
        Show( );
        ImageTile tile = window.Item;
        if (tile?.IsEnabled != true)
            return;
        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }

    private void AddTextTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateText window = new( );
        window.ShowDialog( );
        Show( );
        TextTile tile = window.Item;
        if (tile?.IsEnabled != true)
            return;
        TilePanel.Children.Add(tile);
        tile.Refresh( );
    }
    private void ImportAppTile(object o, RoutedEventArgs e)
    {
        Hide( );
        ImportApp window = new( );
        window.ShowDialog( );
        foreach (AppTile tile in window.Tiles)
        {
            TilePanel.Children.Add(tile);
            tile.IsEnabled = true;
            tile.Refresh( );
        }
        Show( );
    }

    private void LoadBackground( )
    {
        try
        {
            if (App.Settings.Content.Background.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                MainBorder.Background = new ImageBrush(PEIcon.Get(App.Settings.Content.Background)) { Stretch = Stretch.UniformToFill };
            }
            else if (App.Settings.Content.Background.StartsWith('#'))
            {
                int rgb = Convert.ToInt32(App.Settings.Content.Background.Replace("#", ""), 16);
                byte R = (byte) ((rgb >> 16) & 0xFF);
                byte G = (byte) ((rgb >> 8) & 0xFF);
                byte B = (byte) (rgb & 0xFF);
                MainBorder.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            }
            else
            {
                MainBorder.Background = new ImageBrush(new BitmapImage(new Uri(App.Settings.Content.Background))) { Stretch = Stretch.UniformToFill };
            }
        }
        catch { MainBorder.Background = Defaults.Background; }
        MainBorder.Background.Freeze( );
    }
    private void PinApp(object o, RoutedEventArgs e)
    {
        StartMenuApp app = AppList.SelectedItem as StartMenuApp;
        AppTile appTile = new( )
        {
            AppName = app?.AppName,
            AppPath = app?.AppPath,
            AppIcon = app?.AppPath,
            TileSize = TileSize.Medium,
            Row = 0,
            Column = 0
        };
        TilePanel.Children.Add(appTile);
        appTile.Refresh( );
    }

    private void SaveData(object o, RoutedEventArgs e)
    {
        SaveDataButton.IsEnabled = false;
        UpdateGlobalTiles( );
        TileStore.Save( );
        App.Settings.Write( );
        SaveDataButton.Content = "\uE73E";
        Task.Delay(2000).ContinueWith(_ => Dispatcher.Invoke(( ) =>
        {
            SaveDataButton.Content = "\uE78C";
            SaveDataButton.IsEnabled = true;
        }));
    }

    private void ShowHideAppList(object o, RoutedEventArgs e)
    {
        if ((AppListBorder.RenderTransform as TranslateTransform)?.X != 0 && Resources["ShowAppList"] is Storyboard showAnimation)
        {
            AppListBorder.Visibility = Visibility.Visible;
            showAnimation.Begin(AppListBorder);
        }
        else if (AppListBorder.RenderTransform is TranslateTransform { X: 0 } && Resources["HideAppList"] is Storyboard hideAnimation)
        {
            hideAnimation.Completed += (o, e) => AppListBorder.Visibility = Visibility.Collapsed;
            hideAnimation.Begin(AppListBorder);
        }
    }

    private void ShowSetting(object o, RoutedEventArgs e)
    {
        Hide( );
        new Setting( ).ShowDialog( );
        LoadBackground( );
        Show( );
    }

    private void TaskbarMenuExit(object o, RoutedEventArgs e)
        => Application.Current.Shutdown( );

    private void TaskbarMenuShow(object o, RoutedEventArgs e)
        => ShowHide( );

    private void UpdateGlobalTiles( )
    {
        App.Tiles.Clear( );
        foreach (TileBase tile in TilePanel.Children)
            App.Tiles.Add(tile);
    }

    private void WindowClosing(object o, CancelEventArgs e)
    {
        Hide( );
        UpdateGlobalTiles( );
        e.Cancel = true;
    }
    private void WindowDeactivated(object o, EventArgs e)
        => ShowHide( );

    private void WindowExit(object o, RoutedEventArgs e)
    {
        Application.Current.Shutdown( );
    }

    private void WindowLoaded(object o, RoutedEventArgs e)
    {
        TilePanel.ResizeToFit( );
        ShowHideAppList(null, null);
        Task.Factory.StartNew(( ) =>
        {
            StartMenuApp.LoadApps( );
            Dispatcher.BeginInvoke(( ) =>
            {
                StartMenuApp.LoadIcon( );
                AppList.ItemsSource = StartMenuApp.Apps;
            });
        });
    }
}
