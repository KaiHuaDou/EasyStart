using System.Windows;
using System.Windows.Media;

namespace StartPro.Api;

public partial class ColorPickerDialog : Window
{
    public bool IsSelected { get; set; }
    public Color Color => Color.FromArgb(
        (byte) colorPicker.Color.A,
        (byte) colorPicker.Color.RGB_R,
        (byte) colorPicker.Color.RGB_G,
        (byte) colorPicker.Color.RGB_B);

    public ColorPickerDialog( )
    {
        InitializeComponent( );
    }

    private void ColorChanged(object o, RoutedEventArgs e)
    {
        IsSelected = true;
    }
}
