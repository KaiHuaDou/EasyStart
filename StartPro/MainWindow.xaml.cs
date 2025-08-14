using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using NHotkey.Wpf;
using StartPro.Api;
using StartPro.Tile;
using static StartPro.External.NativeMethods;

namespace StartPro;

public partial class MainWindow : Window
{
    private IntPtr handle;

    public MainWindow( )
    {
        InitializeComponent( );

        handle = new WindowInteropHelper(this).Handle;
        HotkeyManager.Current.AddOrReplace(
            "ShowHide",
            new KeyGesture(Key.None, ModifierKeys.Windows | ModifierKeys.Control),
            (_, e) =>
            {
                ShowHide( );
                e.Handled = true;
            }
        );

        Height = Defaults.HeightPercent * SystemParameters.PrimaryScreenHeight;
        Width = Defaults.WidthPercent * SystemParameters.PrimaryScreenWidth;
        TilePanel.MinHeight = Height - 256;
        TilePanel.MinWidth = Width - 96;
        Top = SystemParameters.WorkArea.Height - Height;
        Left = (SystemParameters.WorkArea.Width - Width) / 2;
        LoadSettings( );

        AppList.ItemsSource = new Collection<StartMenuApp>{
            new( ) {
                AppName = "Loading",
                AppPath = "",
                AppIcon = new BitmapImage()
            }
        };

        InfoBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding( ) { Source = App.Infos });
        void UpdateHeader(object? _o, NotifyCollectionChangedEventArgs _e)
        {
            string countText = InfoBox.Items.Count == 0 ? "" : $" ({InfoBox.Items.Count})";
            InfoGroup?.Header = $"信息{countText}";
        }
        App.Infos.CollectionChanged += UpdateHeader;
        UpdateHeader(null, null);

        foreach (TileBase tile in App.Tiles)
        {
            TilePanel.Children.Add(tile);
            tile.Refresh( );
        }
    }

    public new void Hide( )
    {
        if (Resources["HideWindow"] is not Storyboard hideAnimation)
            return;
        hideAnimation.Completed += (o, e) => base.Hide( );
        hideAnimation.Begin(MainBorder);
    }

    public new void Show( )
    {
        if (Resources["ShowWindow"] is not Storyboard showAnimation)
            return;

        base.Show( );
        Activate( );
        SetForegroundWindow(handle);
        showAnimation.Begin(MainBorder);
    }

    public void ShowHide( )
    {
        if (Visibility == Visibility.Hidden)
            Show( );
        else
            Hide( );
    }

    private void ClearInfo(object o, RoutedEventArgs e)
    {
        App.Infos.Remove(InfoBox.Text);
        InfoBox.SelectedIndex = 0;
    }

    private void LoadSettings( )
    {
        MainBorder.Background =
            Utils.TryParseBrushFromText(App.Settings.Background, out Brush back)
            ? back : Defaults.Background;
        foreach (TileBase tile in TilePanel.Children)
        {
            tile.Foreground = Utils.TryParseBrushFromText(App.Settings.Foreground, out Brush fore)
                ? fore : Defaults.Foreground;
        }
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

    private void ShowSetting(object o, RoutedEventArgs e)
    {
        Hide( );
        new Setting( ).ShowDialog( );
        LoadSettings( );
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
        => Hide( );

    private void WindowExit(object o, RoutedEventArgs e)
        => Application.Current.Shutdown( );

    private void WindowLoaded(object o, RoutedEventArgs e)
    {
        TilePanel.ResizeToFit( );
        ShowHideAppList(null, null);
#if !DEBUG
        Task.Factory.StartNew(( ) =>
        {
            StartMenuApp.LoadApps( );
            Dispatcher.BeginInvoke(( ) =>
            {
                StartMenuApp.LoadIcon( );
                AppList.ItemsSource = StartMenuApp.Apps;
            });
        });
#endif
    }
}
