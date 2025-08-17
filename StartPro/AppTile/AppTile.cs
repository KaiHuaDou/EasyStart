using System;
using System.Windows;
using System.Xml;

namespace StartPro.Tile;

public partial class AppTile
{
    public override string ToString( ) => AppName;

    private static readonly PropertyMetadata appNameMeta = new("Application");
    private static readonly PropertyMetadata appIconMeta = new(AppIconChanged);
    private static readonly PropertyMetadata appPathMeta = new(Environment.ProcessPath, AppPathChanged);
    private static readonly PropertyMetadata ImageShadowMeta = new(true, ImageShadowChanged);
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
        set => SetValue(ImageShadowProperty, value);
    }

    public override void WriteAttributes(ref XmlElement element)
    {
        base.WriteAttributes(ref element);
        element.SetAttribute("Type", "AppTile");
        element.SetAttribute("Name", AppName);
        element.SetAttribute("Path", AppPath);
        element.SetAttribute("Icon", AppIcon);
        element.SetAttribute("ImageShadow", ImageShadow.ToString( ));
        element.SetAttribute("FontSize", FontSize.ToString( ));
    }

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        FontSize = double.Parse(node.GetAttribute("FontSize"));
        AppPath = node.GetAttribute("Path");
        AppName = node.GetAttribute("Name");
        AppIcon = node.GetAttribute("Icon");
        ImageShadow = bool.Parse(node.GetAttribute("ImageShadow"));
    }
}
