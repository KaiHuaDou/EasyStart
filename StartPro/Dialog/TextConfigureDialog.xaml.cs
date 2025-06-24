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
        FontFamilyBox.ItemsSource = Fonts.SystemFontFamilies;
        FontSizeBox.Text = TextConfig.FontSize.ToString( );
        FontFamilyBox.SelectedItem = TextConfig.FontFamily;
        FontWeightBox.SelectedIndex = FontWeightBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.FontWeight == TextConfig.FontWeight)
            ?.index ?? -1;
        FontStyleBox.SelectedIndex = FontStyleBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.FontStyle == TextConfig.FontStyle)
            ?.index ?? -1;
        FontStretchBox.SelectedIndex = FontStretchBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.FontStretch == TextConfig.FontStretch)
            ?.index ?? -1;
        TextAlignmentBox.SelectedIndex = TextAlignmentBox.Items
            .Cast<TextBlock>( )
            .Select((item, index) => new { item, index })
            .FirstOrDefault(x => x.item.TextAlignment == TextConfig.TextAlignment)
            ?.index ?? -1; TextDecorationUnderLineBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.Underline[0]);
        TextDecorationOverLineBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.OverLine[0]);
        TextDecorationStrikeThroughBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.Strikethrough[0]);
        TextDecorationBaseLineBlock.IsChecked = TextConfig.TextDecorations.Contains(TextDecorations.Baseline[0]);
        TextShadowBox.IsChecked = TextConfig.TextShadow;
        colorPicker.SelectedColor = TextConfig.TextColor.Color;
    }

    private void TaskCancel(object o, RoutedEventArgs e)
    {
        Close( );
    }

    private void TaskOk(object o, RoutedEventArgs e)
    {
        TextDecorationCollection textDecorations = [];
        if (TextDecorationBaseLineBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.Baseline);
        if (TextDecorationUnderLineBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.Underline);
        if (TextDecorationOverLineBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.OverLine);
        if (TextDecorationStrikeThroughBlock.IsChecked == true)
            textDecorations.Add(TextDecorations.Strikethrough);
        IsSelected = true;
        TextConfig = new TextConfig
        {
            FontSize = Utils.ToFontSize(FontSizeBox.Text),
            FontFamily = (FontFamily) FontFamilyBox.SelectedItem,
            FontWeight = (FontWeightBox.SelectedItem as TextBlock)?.FontWeight ?? Defaults.FontWeight,
            FontStyle = (FontStyleBox.SelectedItem as TextBlock)?.FontStyle ?? Defaults.FontStyle,
            FontStretch = (FontStretchBox.SelectedItem as TextBlock)?.FontStretch ?? Defaults.FontStretch,
            TextDecorations = textDecorations,
            TextAlignment = (TextAlignmentBox.SelectedItem as TextBlock)?.TextAlignment ?? Defaults.TextAlignment,
            TextColor = new SolidColorBrush(colorPicker.SelectedColor),
            TextShadow = TextShadowBox.IsChecked == true
        };
        Close( );
    }
}
