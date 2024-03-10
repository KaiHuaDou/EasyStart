using System.Windows;

namespace StartPro;
/// <summary>
/// QuickStart.xaml 的交互逻辑
/// </summary>
public partial class QuickStart : Window
{
    public QuickStart( )
    {
        InitializeComponent( );
    }

    private void ShowMainwindow(object o, RoutedEventArgs e)
        => App.TileWindow?.Show( );
}
