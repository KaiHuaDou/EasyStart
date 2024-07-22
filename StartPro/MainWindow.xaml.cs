using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using StartPro.Api;
using StartPro.External;
using StartPro.Tile;
using static StartPro.External.NativeMethods;

namespace StartPro;

public partial class MainWindow : Window
{
    public CustomCommand SwitchStateCommand => new(ShowHide);

    public MainWindow( )
    {
        InitializeComponent( );

        Height = Defaults.SizeRate * SystemParameters.PrimaryScreenHeight;
        Width = Defaults.SizeRate * SystemParameters.PrimaryScreenWidth;
        Top = SystemParameters.WorkArea.Height - Height;
        Left = (SystemParameters.WorkArea.Width - Width) / 2;
        ApplyBackground( );

        ShowHideAppList(null, null);
        StartMenuApp.SearchAll( );
        AppList.ItemsSource = StartMenuApp.AllApps;

        foreach (TileBase tile in App.Tiles)
        {
            TilePanel.Children.Add(tile);
        }
        TileBase.ResizeCanvas(TilePanel);
    }

    public void ShowHide( )
    {
        if (Visibility == Visibility.Hidden) Show( );
        else Hide( );
    }

    private void AddTile(object o, RoutedEventArgs e)
    {
        Hide( );
        CreateApp window = new( );
        window.ShowDialog( );
        Show( );
        AppTile tile = window.Item;
        if (!tile.IsEnabled)
            return;
        TilePanel.Children.Add(tile);
        tile.MoveToSpace(TilePanel, true);
    }

    private void ShowSetting(object o, RoutedEventArgs e)
    {
        Hide( );
        new Setting( ).ShowDialog( );
        ApplyBackground( );
        Show( );
    }

    private void ImportTile(object o, RoutedEventArgs e)
    {
        Hide( );
        Import window = new( );
        window.ShowDialog( );
        foreach (AppTile tile in window.Tiles)
        {
            TilePanel.Children.Add(tile);
            tile.IsEnabled = true;
            tile.MoveToSpace(TilePanel, true);
        }
        Show( );
    }

    private void ShowHideAppList(object o, RoutedEventArgs e)
    {
        if ((AppListBorder.RenderTransform as TranslateTransform).X != 0 && Resources["ShowAppList"] is Storyboard showAnimation)
        {
            //AppListBorder.Visibility = Visibility.Visible;
            showAnimation.Begin(AppListBorder);
        }
        else if ((AppListBorder.RenderTransform as TranslateTransform).X == 0 && Resources["HideAppList"] is Storyboard hideAnimation)
        {
            //hideAnimation.Completed += (o, e) => AppListBorder.Visibility = Visibility.Collapsed;
            hideAnimation.Begin(AppListBorder);
        }
    }

    private void ApplyBackground( )
    {
        try
        {
            if (App.Program.Settings.Content.Background.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                MainBorder.Background = new ImageBrush(IconMgr.Get(App.Program.Settings.Content.Background)) { Stretch = Stretch.UniformToFill };
            }
            else if (char.IsLetter(App.Program.Settings.Content.Background[0]))
            {
                MainBorder.Background = new ImageBrush(new BitmapImage(new Uri(App.Program.Settings.Content.Background))) { Stretch = Stretch.UniformToFill };
            }
            else
            {
                int rgb = Convert.ToInt32(App.Program.Settings.Content.Background.Replace("#", ""), 16);
                byte R = (byte) ((rgb >> 16) & 0xFF);
                byte G = (byte) ((rgb >> 8) & 0xFF);
                byte B = (byte) (rgb & 0xFF);
                MainBorder.Background = new SolidColorBrush(Color.FromRgb(R, G, B));
            }
        }
        catch { }
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

    public new void Hide( )
    {
        if (Resources["HideWindow"] is Storyboard hideAnimation)
        {
            hideAnimation.Completed += (o, e) => base.Hide( );
            hideAnimation.Begin(MainBorder);
        }
    }

    private void WindowClosing(object o, CancelEventArgs e)
    {
        Hide( );
        App.Tiles.Clear( );
        foreach (AppTile tile in TilePanel.Children)
            App.Tiles.Add(tile);
        e.Cancel = true;
    }

    private void WindowDeactivated(object o, EventArgs e)
            => ShowHide( );

    private void TaskbarMenuShow(object o, RoutedEventArgs e)
        => ShowHide( );

    private void TaskbarMenuExit(object o, RoutedEventArgs e)
        => Application.Current.Shutdown( );
}
