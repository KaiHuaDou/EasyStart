using System.Windows;
using System.Windows.Controls;
using System.Xml;
using StartPro.Api;

namespace StartPro.Tile;

public partial class AppTile : TileBase
{
    public AppTile( ) : base( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null;
        border.Child = RootPanel;

        Utils.AppendContexts(ContextMenu, contextMenu);
        Content = root;
    }
    public override string ToString( ) => $"{AppName} - {TileSize}";

    private static readonly PropertyMetadata appNameMeta = new("Application");
    private static readonly PropertyMetadata appIconMeta = new(AppIconChanged);
    private static readonly PropertyMetadata appPathMeta = new(Defaults.AppName, AppPathChanged);
    private static readonly PropertyMetadata ImageShadowMeta = new(true);
    public static readonly DependencyProperty AppNameProperty
        = DependencyProperty.Register("AppName", typeof(string), typeof(AppTile), appNameMeta);
    public static readonly DependencyProperty AppIconProperty
        = DependencyProperty.Register("AppIcon", typeof(string), typeof(AppTile), appIconMeta);
    public static readonly DependencyProperty AppPathProperty
        = DependencyProperty.Register("AppPath", typeof(string), typeof(AppTile), appPathMeta);
    public static readonly DependencyProperty ImageShadowProperty
        = DependencyProperty.Register("TileImageShadow", typeof(bool), typeof(AppTile), ImageShadowMeta);

    public string AppName
    {
        get => (string) GetValue(AppNameProperty);
        set => SetValue(AppNameProperty, value);
    }

    public string AppIcon
    {
        get => (string) GetValue(AppIconProperty);
        set => SetValue(AppIconProperty, value);
    }

    public string AppPath
    {
        get => (string) GetValue(AppPathProperty);
        set => SetValue(AppPathProperty, value);
    }

    public bool ImageShadow
    {
        get => (bool) GetValue(ImageShadowProperty);
        set
        {
            SetValue(ImageShadowProperty, value);
            TileImageShadow.Opacity = (!App.Program.Settings.Content.UIFlat && value) ? 0.4 : 0;
        }
    }

    public override void WriteAttributes(ref XmlElement element)
    {
        base.WriteAttributes(ref element);
        element.SetAttribute("Type", "AppTile");
        element.SetAttribute("Name", AppName);
        element.SetAttribute("Path", AppPath);
        element.SetAttribute("Icon", AppIcon);
        element.SetAttribute("Shadow", Shadow.ToString( ));
        element.SetAttribute("ImageShadow", ImageShadow.ToString( ));
        element.SetAttribute("FontSize", FontSize.ToString( ));
    }

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        AppPath = node.GetAttribute("Path");
        AppName = node.GetAttribute("Name");
        AppIcon = node.GetAttribute("Icon");
        Shadow = bool.Parse(node.GetAttribute("Shadow"));
        ImageShadow = bool.Parse(node.GetAttribute("ImageShadow"));
    }
}
