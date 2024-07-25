using System.Windows;

namespace StartPro;

public partial class Launcher : Window
{
    public Launcher( )
    {
        InitializeComponent( );
    }

    private void ShowMainwindow(object o, RoutedEventArgs e)
        => App.TileWindow?.ShowHide( );
}
