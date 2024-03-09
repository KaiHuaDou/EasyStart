using System;
using System.Windows;
using System.Windows.Interop;
using static StartPro.External.NativeMethods;

namespace StartPro;
public partial class MainWindow : Window
{
    private IntPtr handle;
    private HwndSource hwndSource;
    private const int HOTKEY_ID = 9527;
    private const int MOD_CONTROL = 0x2;
    private const int KEY_F3 = 0x72;

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
}
