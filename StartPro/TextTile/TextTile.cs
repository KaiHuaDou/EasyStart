using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using StartPro.Api;

namespace StartPro.Tile;
public partial class TextTile
{
    public TextTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null;
        border.Child = TextField;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;
    }

    public override string ToString( ) => $"{Text} - {TileSize}";

    private static readonly PropertyMetadata textMeta = new("Text");
    private static readonly PropertyMetadata textShadowMeta = new(true, TextShadowChanged);
    private static readonly PropertyMetadata textVerticalAlignmentMeta = new(VerticalAlignment.Center);
    private static readonly PropertyMetadata textHorizontalAlignmentMeta = new(HorizontalAlignment.Center);
    public static readonly DependencyProperty TextProperty
        = DependencyProperty.Register("Text", typeof(string), typeof(TextTile), textMeta);
    public static readonly DependencyProperty TextShadowProperty
        = DependencyProperty.Register("TextShadow", typeof(bool), typeof(TextTile), textShadowMeta);
    public static readonly DependencyProperty TextVerticalAlignmentProperty
        = DependencyProperty.Register("TextVerticalAlignment", typeof(VerticalAlignment), typeof(TextTile), textVerticalAlignmentMeta);
    public static readonly DependencyProperty TextHorizontalAlignmentProperty
        = DependencyProperty.Register("TextHorizontalAlignment", typeof(HorizontalAlignment), typeof(TextTile), textHorizontalAlignmentMeta);

    public string Text
    {
        get => (string) GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool TextShadow
    {
        get => (bool) GetValue(TextShadowProperty);
        set => SetValue(TextShadowProperty, value);
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
        element.SetAttribute("TextShadow", TextShadow.ToString( ));
        element.SetAttribute("TextVerticalAlignment", ((int) TextVerticalAlignment).ToString( ));
        element.SetAttribute("TextHorizontalAlignment", ((int) TextHorizontalAlignment).ToString( ));
        element.SetAttribute("FontStyle", (FontStyle == FontStyles.Italic).ToString( ));
        element.SetAttribute("FontWeight", (FontWeight == FontWeights.Bold).ToString( ));
        element.SetAttribute("FontFamily", FontFamily.ToString( ));
        element.SetAttribute("FontSize", FontSize.ToString( ));
        return element;
    }

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        Text = node.GetAttribute("Text");
        TextShadow = bool.Parse(node.GetAttribute("TextShadow"));
        TextVerticalAlignment = (VerticalAlignment) int.Parse(node.GetAttribute("TextVerticalAlignment"));
        TextHorizontalAlignment = (HorizontalAlignment) int.Parse(node.GetAttribute("TextHorizontalAlignment"));
        FontStyle = bool.Parse(node.GetAttribute("FontStyle")) ? FontStyles.Italic : FontStyles.Normal;
        FontWeight = bool.Parse(node.GetAttribute("FontWeight")) ? FontWeights.Bold : FontWeights.Normal;
        FontFamily = new FontFamily(node.GetAttribute("FontFamily"));
        FontSize = double.Parse(node.GetAttribute("FontSize"));
    }
}
