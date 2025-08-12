using System.Windows;
using System.Windows.Media;

namespace StartPro.Api;

public partial class ColorDialog : Window
{
    public bool IsSelected { get; private set; }

    public Color Color
    {
        get => colorPicker.SelectedColor;
        set => colorPicker.SelectedColor = value;
    }

    public ColorDialog( )
    {
        InitializeComponent( );
    }

    private void TaskCancel(object o, RoutedEventArgs e)
    {
        Close( );
    }

    private void TaskOk(object o, RoutedEventArgs e)
    {
        IsSelected = true;
        Close( );
    }
}
