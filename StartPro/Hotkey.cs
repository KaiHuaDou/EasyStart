using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using StartPro.Resources;
using static StartPro.External.NativeMethods;

namespace StartPro;

public partial class MainWindow
{
    private IntPtr handle;
    private HwndSource hwndSource;
    private const int HOTKEY_ID = 9527;
    private const int KEY_F3 = 0x72;


    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        handle = new WindowInteropHelper(this).Handle;
        hwndSource = HwndSource.FromHwnd(handle);
        hwndSource.AddHook(HwndHook);
        bool hresult = RegisterHotKey(handle, HOTKEY_ID, KeyModifiers.WindowsKey, KEY_F3);
        if (!hresult)
        {
            if (Marshal.GetLastWin32Error( ) == 0x581)
            {
                MessageBox.Show(Main.HotkeyUsed);
            }
            else
            {
                MessageBox.Show(Main.HotkeyRegisterFailed);
            }
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        hwndSource.RemoveHook(HwndHook);
        UnregisterHotKey(handle, HOTKEY_ID);
    }

    public IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WM_HOTKEY && wParam.ToInt32( ) == HOTKEY_ID)
            ShowHide( );
        return IntPtr.Zero;
    }
}
