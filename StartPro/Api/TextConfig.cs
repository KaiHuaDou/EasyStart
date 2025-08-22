using System.Windows;
using System.Windows.Media;
using System.Xml;
using StartPro.Converter;
using StartPro.Tile;

namespace StartPro.Api;

public class TextConfig : IStorable
{
    public double FontSize { get; set; } = Defaults.FontSize;
    public FontFamily FontFamily { get; set; } = Defaults.FontFamily;
    public FontWeight FontWeight { get; set; } = Defaults.FontWeight;
    public FontStyle FontStyle { get; set; } = Defaults.FontStyle;
    public FontStretch FontStretch { get; set; } = Defaults.FontStretch;
    public TextDecorationCollection TextDecorations { get; set; } = [];
    public TextAlignment TextAlignment { get; set; } = Defaults.TextAlignment;
    public bool TextShadow { get; set; } = true;
    public SolidColorBrush TextColor { get; set; } = Defaults.Foreground;

    public void ReadAttributes(XmlNode node)
    {
        FontSize = node.FromAttribute("FontSize", Defaults.FontSize);
        FontFamily = node.FromAttribute("FontFamily", Defaults.FontFamily);
        FontWeight = node.FromAttribute("FontWeight", Defaults.FontWeight);
        FontStyle = node.FromAttribute("FontStyle", Defaults.FontStyle);
        FontStretch = node.FromAttribute("FontStretch", Defaults.FontStretch);
        TextDecorations = node.FromAttribute("TextDecoration", new TextDecorationCollection( ));
        TextAlignment = node.FromAttribute("TextAlignment", Defaults.TextAlignment);
        TextShadow = node.FromAttribute("TextShadow", false);
        TextColor = new SolidColorBrush(node.FromAttribute("TextColor", Defaults.ForegroundColor));
    }

    public void WriteAttributes(ref XmlElement element)
    {
        element.SetAttribute("FontSize", FontSize.ToString( ));
        element.SetAttribute("FontFamily", FontFamily.ToString( ));
        element.SetAttribute("FontWeight", FontWeight.ToString( ));
        element.SetAttribute("FontStyle", FontStyle.ToString( ));
        element.SetAttribute("FontStretch", FontStretch.ToString( ));
        element.SetAttribute("TextDecoration", TextDecorations.ConvertToString( ));
        element.SetAttribute("TextAlignment", TextAlignment.ToString( ));
        element.SetAttribute("TextShadow", TextShadow.ToString( ));
        element.SetAttribute("TextColor", TextColor.ToString( ));
    }
}
