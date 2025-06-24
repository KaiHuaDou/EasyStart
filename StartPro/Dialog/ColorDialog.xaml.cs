using System.Windows;
using System.Windows.Media;

namespace StartPro.Api;

public partial class ColorDialog : Window
{
    public bool IsSelected { get; private set; }
    public Color Color => colorPicker.SelectedColor;

    public ColorDialog( )
    {
        InitializeComponent( );
    }
}
