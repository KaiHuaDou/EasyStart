using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using StartPro.External;
using static StartPro.External.NativeMethods;

namespace StartPro;

public partial class MainWindow : Window
{
    public CustomCommand SwitchStateCommand => new(( ) => SwitchState( ));

    public MainWindow( )
    {
        InitializeComponent( );
        Height = Defaults.SizeRate * SystemParameters.PrimaryScreenHeight;
        Width = Defaults.SizeRate * SystemParameters.PrimaryScreenWidth;
        Top = SystemParameters.WorkArea.Height - Height;
        Left = (SystemParameters.WorkArea.Width - Width) / 2;
        ApplyBackground( );
        foreach (Tile tile in App.Tiles)
        {
            TilePanel.Children.Add(tile);
        }
        Tile.ResizeCanvas(TilePanel);
    }

    public void SwitchState(bool? mode = null)
    {
        switch (mode)
        {
            case null: SwitchState(Visibility == Visibility.Hidden); break;
            case true: Show( ); break;
            case false: Hide( ); break;
        }
    }

    private void AddTile(object o, RoutedEventArgs e)
    {
        Hide( );
        Create window = new( );
        window.ShowDialog( );
        Show( );
        Tile tile = window.Item;
        if (!tile.IsEnabled)
            return;
        TilePanel.Children.Add(tile);
        tile.MoveToSpace(TilePanel, false);
    }

    private void ShowSetting(object o, RoutedEventArgs e)
    {
        Hide( );
        new Setting( ).ShowDialog( );
        ApplyBackground( );
        Show( );
    }

    private void ApplyBackground( )
    {
        try
        {
            if (char.IsLetter(App.Setting.Content.Background[0]))
            {
                MainBorder.Background = new ImageBrush(new BitmapImage(new Uri(App.Setting.Content.Background)));
            }
            else
            {
                int rgb = Convert.ToInt32(App.Setting.Content.Background.Replace("#", ""), 16);
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
            showAnimation.Begin(this);
        }
        SetForegroundWindow(handle);
    }

    public new void Hide( )
    {
        if (Resources["HideWindow"] is Storyboard hideAnimation)
        {
            hideAnimation.Completed += (o, e) => base.Hide( );
            hideAnimation.Begin(this);
        }
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
