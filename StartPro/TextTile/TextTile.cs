using System.Windows;
using System.Xml;
using StartPro.Api;

namespace StartPro.Tile;
public partial class TextTile
{
    public override string ToString( ) => $"{Text}";

    private static readonly PropertyMetadata textMeta = new("Text");
    private static readonly PropertyMetadata textConfigMeta = new(new TextConfig( ), TextConfigChanged);
    private static readonly PropertyMetadata textVerticalAlignmentMeta = new(VerticalAlignment.Center);
    private static readonly PropertyMetadata textHorizontalAlignmentMeta = new(HorizontalAlignment.Center);
    public static readonly DependencyProperty TextProperty
        = DependencyProperty.Register("Text", typeof(string), typeof(TextTile), textMeta);
    public static readonly DependencyProperty TextConfigProperty
        = DependencyProperty.Register("TextConfig", typeof(TextConfig), typeof(TextTile), textConfigMeta);
    public static readonly DependencyProperty TextVerticalAlignmentProperty
        = DependencyProperty.Register("TextVerticalAlignment", typeof(VerticalAlignment), typeof(TextTile), textVerticalAlignmentMeta);
    public static readonly DependencyProperty TextHorizontalAlignmentProperty
        = DependencyProperty.Register("TextHorizontalAlignment", typeof(HorizontalAlignment), typeof(TextTile), textHorizontalAlignmentMeta);

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public TextConfig TextConfig
    {
        get => (TextConfig) GetValue(TextConfigProperty);
        set => SetValue(TextConfigProperty, value);
    }

    public VerticalAlignment TextVerticalAlignment
    {
        get => (VerticalAlignment) GetValue(TextVerticalAlignmentProperty);
        set => SetValue(TextVerticalAlignmentProperty, value);
    }

    public HorizontalAlignment TextHorizontalAlignment
    {
        get => (HorizontalAlignment) GetValue(TextHorizontalAlignmentProperty);
        set => SetValue(TextHorizontalAlignmentProperty, value);
    }

    public override XmlElement WriteAttributes(XmlElement element)
    {
        element = base.WriteAttributes(element);
        element.SetAttribute("Type", "TextTile");
        element.SetAttribute("Text", Text);
        element.SetAttribute("TextConfig", TextConfig.ToString( ));
        element.SetAttribute("TextVerticalAlignment", ((int) TextVerticalAlignment).ToString( ));
        element.SetAttribute("TextHorizontalAlignment", ((int) TextHorizontalAlignment).ToString( ));
        return element;
    }

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        Text = node.GetAttribute("Text");
        TextConfig = TextConfig.FromString(node.GetAttribute("TextConfig"));
        TextVerticalAlignment = (VerticalAlignment) int.Parse(node.GetAttribute("TextVerticalAlignment"));
        TextHorizontalAlignment = (HorizontalAlignment) int.Parse(node.GetAttribute("TextHorizontalAlignment"));
    }
}
