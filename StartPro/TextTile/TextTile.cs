using System.Windows;

namespace StartPro.Tile;
public partial class TextTile
{
    public override string ToString( ) => $"{Text} - {TileSize}";

    private static readonly PropertyMetadata textMeta = new("Text");
    private static readonly PropertyMetadata textShadowMeta = new(true);
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextTile), textMeta);
    public static readonly DependencyProperty TextShadowProperty = DependencyProperty.Register("TextShadow", typeof(bool), typeof(TextTile), textShadowMeta);

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
}
