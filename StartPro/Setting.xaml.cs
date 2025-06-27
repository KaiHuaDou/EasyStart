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
            Main.ResourceManager.GetString("UITheme.Aero.NormalColor") + " (Aero.NormalColor)",
            Main.ResourceManager.GetString("UITheme.Aero2.NormalColor") + " (Aero2.NormalColor)",
            Main.ResourceManager.GetString("UITheme.Luna.NormalColor") + " (Luna.NormalColor)",
            Main.ResourceManager.GetString("UITheme.Luna.Homestead") + " (Luna.Homestead)",
            Main.ResourceManager.GetString("UITheme.Luna.Metallic") + " (Luna.Metallic)",
            Main.ResourceManager.GetString("UITheme.Royale.NormalColor") + " (Royale.NormalColor)",
            Main.ResourceManager.GetString("UITheme.Classic") + " (Classic)",
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
        if (Utils.TrySelectFile(out string fileName, ".jpg"))
            BackgroundBox.Text = fileName;
    }
}
