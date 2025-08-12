using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using StartPro.Api;
using StartPro.Resources;

namespace StartPro;
public partial class Setting : Window
{
    public Setting( )
    {
        InitializeComponent( );
        List<string> UIThemeSource =
        [
            Main.UITheme_Aero_NormalColor + " (Aero.NormalColor)",
            Main.UITheme_Aero2_NormalColor + " (Aero2.NormalColor)",
            Main.UITheme_Luna_NormalColor + " (Luna.NormalColor)",
            Main.UITheme_Luna_Homestead + " (Luna.Homestead)",
            Main.UITheme_Luna_Metallic + " (Luna.Metallic)",
            Main.UITheme_Royale_NormalColor + " (Royale.NormalColor)",
            Main.UITheme_Classic + " (Classic)",
        ];
        MaxWidth = Defaults.WidthPercent * SystemParameters.PrimaryScreenWidth;
        BackgroundBox.Text = App.Settings.Content.Background;
        UIThemeBox.ItemsSource = UIThemeSource;
        UIThemeBox.SelectedIndex = App.Settings.Content.UITheme;
        UIFlatBox.IsChecked = App.Settings.Content.UIFlat;
    }

    private void CancelClick(object o, RoutedEventArgs e)
        => Close( );

    private void OkClick(object o, RoutedEventArgs e)
    {
        App.Settings.Content = new Config
        {
            Background = BackgroundBox.Text,
            UITheme = UIThemeBox.SelectedIndex,
            UIFlat = UIFlatBox.IsChecked == true
        };
        App.Settings.Write( );
        Close( );
    }

    private void SelectColorClick(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectColor(Defaults.Background.Color, out Color color, this))
            BackgroundBox.Text = color.ToString( );
    }

    private void SelectImageClick(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectFile(out string fileName, "img"))
            BackgroundBox.Text = fileName;
    }
}
