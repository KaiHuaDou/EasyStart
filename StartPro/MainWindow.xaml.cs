using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using static StartPro.NativeMethods;

namespace StartPro;

public partial class MainWindow : Window
{
    private IntPtr handle;
    private HwndSource hwndSource;
    private const int HOTKEY_ID = 9527;
    private const int MOD_CONTROL = 0x2;
    private const int KEY_F3 = 0x72;

    public MainWindow( )
    {
        InitializeComponent( );
        Height = Defaults.SizeRate * SystemParameters.PrimaryScreenHeight;
        Width = Defaults.SizeRate * SystemParameters.PrimaryScreenWidth;
        Top = SystemParameters.WorkArea.Height - Height;
        Left = (SystemParameters.WorkArea.Width - Width) / 2;
        foreach (Tile tile in App.Tiles)
        {
            TilePanel.Children.Add(tile);
        }
        Tile.ResizeCanvas(TilePanel);
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        handle = new WindowInteropHelper(this).Handle;
        hwndSource = HwndSource.FromHwnd(handle);
        hwndSource.AddHook(HwndHook);
        RegisterHotKey(handle, HOTKEY_ID, MOD_CONTROL, KEY_F3);

    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        hwndSource.RemoveHook(HwndHook);
        UnregisterHotKey(handle, HOTKEY_ID);
    }

    public IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        const int WM_HOTKEY = 0x0312;
        if (msg == WM_HOTKEY && wParam.ToInt32( ) == HOTKEY_ID)
            SwitchState( );
        return IntPtr.Zero;
    }

    public void SwitchState(bool? mode = null)
    {
        switch (mode)
        {
            case null:
            {
                SwitchState(Visibility == Visibility.Hidden);
                break;
            }
            case true:
            {
                if (Resources["ShowWindow"] is Storyboard showAnimation)
                {
                    Show( );
                    showAnimation.Begin(this);
                }
                SetForegroundWindow(handle);
                break;
            }
            case false:
            {
                if (Resources["HideWindow"] is Storyboard hideAnimation)
                {
                    hideAnimation.Completed += (o, e) => Hide( );
                    hideAnimation.Begin(this);
                }
                break;
            }
        }
    }

    private void AddTile(object o, RoutedEventArgs e)
    {
        Create window = new( );
        window.ShowDialog( );
        Tile tile = window.tile;
        if (!tile.IsEnabled) return;
        TilePanel.Children.Add(tile);
        tile.MoveToSpace(TilePanel, false);
        Show( );
    }

    private void WindowClosing(object o, CancelEventArgs e)
    {
        Hide( );
        App.Tiles.Clear( );
        foreach (Tile tile in TilePanel.Children)
        {
            tile.Init( );
            App.Tiles.Add(tile);
        }
        e.Cancel = true;
    }

    private void WindowDeactivated(object o, EventArgs e)
        => SwitchState( );

    private void TaskbarMenuShow(object o, RoutedEventArgs e)
        => SwitchState( );

    private void TaskbarMenuExit(object o, RoutedEventArgs e)
        => Application.Current.Shutdown( );
}
