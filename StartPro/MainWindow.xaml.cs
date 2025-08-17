using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using StartPro.Api;
using StartPro.Tile;

namespace StartPro;

public partial class MainWindow : Window
{
    public MainWindow( )
    {
        InitializeComponent( );

        Utils.RegisterHotkey(ShowHide);

        Height = Defaults.HeightPercent * SystemParameters.WorkArea.Height;
        Width = Defaults.WidthPercent * SystemParameters.WorkArea.Width;
        Top = SystemParameters.WorkArea.Height - Height;
        Left = (SystemParameters.WorkArea.Width - Width) / 2;
        TilePanel.MinHeight = Height - 256;
        TilePanel.MinWidth = Width - 96;

        AddTiles(App.Tiles);

        ApplySettings( );

        AppList.ItemsSource = new Collection<SystemApp>{
            new( ) {
                AppName = "Loading",
                AppPath = "",
                AppIcon = new BitmapImage()
            }
        };
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
        showAnimation.Begin(MainBorder);
    }

    public void ShowHide( )
    {
        if (Visibility == Visibility.Hidden)
            Show( );
        else
            Hide( );
    }

    private void AddTiles(ObservableCollection<TileBase> tiles)
    {
        foreach (TileBase tile in tiles)
        {
            TilePanel.Children.Add(tile);
            tile.Refresh( );
        }
    }

    private void ApplySettings( )
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

    private void ClearInfo(object o, RoutedEventArgs e)
    {
        App.Infos.Remove(InfoBox.Text);
        InfoBox.SelectedIndex = 0;
    }

    private void ImportSystemStart(object o, RoutedEventArgs e)
    {
        ObservableCollection<TileBase> tiles = SystemTiles.Import( );
        AddTiles(tiles);
    }

    private void InitInfoBox( )
    {
        InfoBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding( ) { Source = App.Infos });
        InfoBox.MaxHeight = FunctionPanel.ActualHeight;
        InfoBox.MaxWidth = 0.25 * SystemParameters.WorkArea.Width;
        void UpdateHeader(object? _o, NotifyCollectionChangedEventArgs _e)
        {
            string countText = InfoBox.Items.Count == 0 ? "" : $" ({InfoBox.Items.Count})";
            InfoGroup?.Header = $"信息{countText}";
        }
        App.Infos.CollectionChanged += UpdateHeader;
        UpdateHeader(null, null);
    }

    private void OpenConfigFolder(object o, RoutedEventArgs e)
    {
        Utils.ExecuteAsAdmin("explorer.exe", $"/e, /select, {Environment.ProcessPath}");
    }

    private void SaveData(object o, RoutedEventArgs e)
    {
        SaveDataButton.IsEnabled = false;
        SyncGlobalTiles( );
        if (TileStore.Save( ) && App.Settings.Write( ))
        {
            SaveDataButton.Content = "\uE73E";
            Task.Delay(2000).ContinueWith(_ => Dispatcher.Invoke(( ) =>
            {
                SaveDataButton.Content = "\uE78C";
                SaveDataButton.IsEnabled = true;
            }));
        }
        else
        {
            SaveDataButton.IsEnabled = true;
        }
    }

    private void ShowSetting(object o, RoutedEventArgs e)
    {
        Hide( );
        new Setting( ).ShowDialog( );
        ApplySettings( );
        Show( );
    }

    private void SyncGlobalTiles( )
    {
        App.Tiles.Clear( );
        foreach (TileBase tile in TilePanel.Children)
            App.Tiles.Add(tile);
    }

    private void TaskbarMenuExit(object o, RoutedEventArgs e)
            => Application.Current.Shutdown( );

    private void TaskbarMenuShow(object o, RoutedEventArgs e)
        => ShowHide( );
    private void WindowClosing(object o, CancelEventArgs e)
    {
        Hide( );
        SyncGlobalTiles( );
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
        InitInfoBox( );

#if !DEBUG
        App.AddInfo("调试模式不加载开始菜单");
#else
        Task.Factory.StartNew(( ) =>
        {
            SystemApp.LoadApps( );
            Dispatcher.BeginInvoke(( ) =>
            {
                SystemApp.LoadIcon( );
                AppList.ItemsSource = SystemApp.Apps;
            });
        });
#endif
    }
}
