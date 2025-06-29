using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StartPro.Api;

public partial class TextConfigureDialog : Window
{
    public bool IsSelected { get; set; }
    public TextConfig TextConfig { get; set; } = new( );

    public TextConfigureDialog( )
    {
        InitializeComponent( );
    }

    public TextConfigureDialog(TextConfig textConfig) : this( )
    {
        TextConfig = textConfig;
        fontFamilyBox.ItemsSource = Fonts.SystemFontFamilies;
        fontSizeBox.Text = TextConfig.FontSize.ToString( );
        fontFamilyBox.SelectedItem = TextConfig.FontFamily;
        fontWeightBox.SelectedIndex = fontWeightBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.FontWeight == TextConfig.FontWeight)
            ?.index ?? -1;
        fontStyleBox.SelectedIndex = fontStyleBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.FontStyle == TextConfig.FontStyle)
            ?.index ?? -1;
        fontStretchBox.SelectedIndex = fontStretchBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.FontStretch == TextConfig.FontStretch)
            ?.index ?? -1;
        textAlignmentBox.SelectedIndex = textAlignmentBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.TextAlignment == TextConfig.TextAlignment)
            ?.index ?? -1; TextDecorationUnderLineBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.Underline[0]);
        textDecorationOverLineBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.OverLine[0]);
        textDecorationStrikeThroughBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.Strikethrough[0]);
        textDecorationBaseLineBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.Baseline[0]);
        textShadowBox.IsChecked = TextConfig.TextShadow;
        colorPicker.SelectedColor = TextConfig.TextColor.Color;
    }

    private void TaskCancel(object o, RoutedEventArgs e)
    {
        Close( );
    }

    private void TaskOk(object o, RoutedEventArgs e)
    {
        TextDecorationCollection textDecorations = [];
        if (textDecorationBaseLineBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.Baseline);
        if (TextDecorationUnderLineBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.Underline);
        if (textDecorationOverLineBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.OverLine);
        if (textDecorationStrikeThroughBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.Strikethrough);
        IsSelected = true;
        TextConfig = new TextConfig
        {
            FontSize = Utils.ToFontSize(fontSizeBox.Text),
            FontFamily = (FontFamily) fontFamilyBox.SelectedItem,
            FontWeight = (fontWeightBox.SelectedItem as TextBlock)?.FontWeight ?? Defaults.FontWeight,
            FontStyle = (fontStyleBox.SelectedItem as TextBlock)?.FontStyle ?? Defaults.FontStyle,
            FontStretch = (fontStretchBox.SelectedItem as TextBlock)?.FontStretch ?? Defaults.FontStretch,
            TextDecorations = textDecorations,
            TextAlignment = (textAlignmentBox.SelectedItem as TextBlock)?.TextAlignment ?? Defaults.TextAlignment,
            TextColor = new SolidColorBrush(colorPicker.SelectedColor),
            TextShadow = textShadowBox.IsChecked == true
        };
        Close( );
    }
}
