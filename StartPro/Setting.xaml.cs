using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StartPro.Api;
using StartPro.Resources;

namespace StartPro;
public partial class Setting : Window
{
    public Setting( )
    {
        InitializeComponent( );
        MaxWidth = Defaults.WidthPercent * SystemParameters.PrimaryScreenWidth;
        BackgroundBox.Text = App.Settings.Content.Background;
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

    private void UIThemeBoxSelectionChanged(object o, SelectionChangedEventArgs e)
    {
        if (UIThemeText is null || UIThemeBox is null) return;
        UIThemeText.Content = Main.ResourceManager.GetString($"UITheme.{UIThemeBox.SelectedValue}");
    }

    private void WindowLoaded(object o, RoutedEventArgs e)
        => UIThemeBoxSelectionChanged(null, null);
}
