using System.Windows;

namespace StartPro.Tile;
public partial class TextTile
{
    public override string ToString( ) => $"{Text} - {TileSize}";

    private static readonly PropertyMetadata textMeta = new("Text");
    private static readonly PropertyMetadata textShadowMeta = new(true);
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
        set
        {
            SetValue(TextShadowProperty, value);
            TileTextShadow.Opacity = (!App.Program.Settings.Content.UIFlat && value) ? 0.4 : 0;
        }
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
}
