using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

using Windows.Win32.System.Shutdown;

using StartPro.Api;
using StartPro.Resources;
using StartPro.Tile;

using static StartPro.External.Power;
using static Windows.Win32.PInvoke;

namespace StartPro;

public partial class MainWindow : Window
{
    public MainWindow( )
    {
        InitializeComponent( );

        Integration.RegisterHotkey(ShowHide);

        Height = Defaults.HeightPercent * SystemParameters.WorkArea.Height;
        Width = Defaults.WidthPercent * SystemParameters.WorkArea.Width;
        Top = SystemParameters.WorkArea.Height - Height;
        Left = (SystemParameters.WorkArea.Width - Width) / 2;
        TilePanel.MinHeight = Height - 256;
        TilePanel.MinWidth = Width - 96;
        InfoBox.MaxHeight = SystemParameters.WorkArea.Height - 256;

        AddTiles(App.Tiles);

        ApplySettings( );

        AppList.ItemsSource = new Collection<SystemApp>{
            new( ) {
                AppName = Info.Loading,
                AppPath = "",
                AppIcon = new BitmapImage()
            }
        };
    }

    public new void Hide( )
    {
        if (Resources["HideWindow"] is not Storyboard hideAnimation)
        {
            return;
        }

        hideAnimation.Completed += (o, e) => base.Hide( );
        hideAnimation.Begin(MainBorder);
    }

    public new void Show( )
    {
        if (Resources["ShowWindow"] is not Storyboard showAnimation)
        {
            return;
        }

        base.Show( );
        Activate( );
        showAnimation.Begin(MainBorder);
    }

    public void ShowHide( )
    {
        if (Visibility == Visibility.Hidden)
        {
            Show( );
        }
        else
        {
            Hide( );
        }
    }

    private void ApplySettings( )
    {
        MainBorder.Background =
            Utils.TryParseBrush(App.Settings.Background, out var back)
            ? back : Defaults.Background;
        foreach (TileBase tile in TilePanel.Children)
        {
            tile.Foreground = Utils.TryParseBrush(App.Settings.Foreground, out var fore)
                ? fore : Defaults.Foreground;
        }
    }

    private void ClearInfo(object o, RoutedEventArgs e)
    {
        if (InfoBox.SelectedValue is string selectedInfo)
        {
            App.Infos.Remove(selectedInfo);
        }

        InfoBox.SelectedIndex = 0;
    }

    private void InitInfoBox( )
    {
        InfoBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding( ) { Source = App.Infos });
        InfoBox.MaxHeight = FunctionPanel.ActualHeight;
        InfoBox.MaxWidth = 0.25 * SystemParameters.WorkArea.Width;
        void UpdateHeader(object? _o, NotifyCollectionChangedEventArgs? _e)
        {
            if (InfoBox.Items.Count == 0)
            {
                InfoGroup.Visibility = Visibility.Collapsed;
            }
            else
            {
                InfoGroup.Header = $"信息 ({InfoBox.Items.Count})";
                InfoGroup.Visibility = Visibility.Visible;
            }
        }

        App.Infos.CollectionChanged += UpdateHeader;
        UpdateHeader(null, null);
    }

    private void OpenConfigFolder(object o, RoutedEventArgs e)
    {
        Integration.ExecuteAsAdmin("explorer.exe", $"/e, /select, {Path.Join(Utils.AppPath, "tiles.xml")}");
    }

    private void PowerLock(object o, RoutedEventArgs e)
    {
        if (!LockWorkStation( ))
        {
            App.AddInfo(Info.LockFailed);
        }
    }

    private void PowerLogout(object o, RoutedEventArgs e)
    {
        if (!ExitWindowsEx(EXIT_WINDOWS_FLAGS.EWX_LOGOFF | EXIT_WINDOWS_FLAGS.EWX_FORCE, 0))
        {
            App.AddInfo(Info.LogoutFailed);
        }
    }

    private void PowerRestart(object o, RoutedEventArgs e)
    {
        if (EnableShutdownPrivilege( ))
        {
            ExitWindowsEx(EXIT_WINDOWS_FLAGS.EWX_REBOOT | EXIT_WINDOWS_FLAGS.EWX_FORCE, 0);
        }
        else
        {
            App.AddInfo(Info.RestartFailed);
        }
    }

    private void PowerShutdown(object o, RoutedEventArgs e)
    {
        if (EnableShutdownPrivilege( ))
        {
            ExitWindowsEx(EXIT_WINDOWS_FLAGS.EWX_SHUTDOWN | EXIT_WINDOWS_FLAGS.EWX_FORCE, 0);
        }
        else
        {
            App.AddInfo(Info.ShutdownFailed);
        }
    }

    private void PowerSleep(object o, RoutedEventArgs e)
    {
        if (!SetSuspendState(false, true, false))
        {
            App.AddInfo(Info.SleepFailed);
        }
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

    private void TaskbarMenuExit(object o, RoutedEventArgs e)
    {
        Application.Current.Shutdown( );
    }

    private void TaskbarMenuShow(object o, RoutedEventArgs e)
    {
        ShowHide( );
    }

    private void WindowClosing(object o, CancelEventArgs e)
    {
        Hide( );
        SyncGlobalTiles( );
        e.Cancel = true;
    }

    private void WindowDeactivated(object o, EventArgs e)
    {
        Hide( );
    }

    private void WindowExit(object o, RoutedEventArgs e)
    {
        Application.Current.Shutdown( );
    }

    private async void WindowLoaded(object o, RoutedEventArgs e)
    {
        TilePanel.ResizeToFit( );
        SwitchAppList(null, null);
        InitInfoBox( );

#if DEBUG
        App.AddInfo(Info.DebugModeNoLoad);
#else
        await Task.Run(( ) =>
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
