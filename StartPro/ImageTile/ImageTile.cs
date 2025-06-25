using System;
using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace StartPro.Tile;
public partial class ImageTile
{
    private static readonly PropertyMetadata imagePathMeta = new("", ImagePathChanged);
    private static readonly PropertyMetadata stretchMeta = new(Stretch.Uniform);
    public static readonly DependencyProperty ImagePathProperty
        = DependencyProperty.Register("ImagePath", typeof(string), typeof(ImageTile), imagePathMeta);
    public static readonly DependencyProperty StretchProperty
        = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageTile), stretchMeta);

    public string ImagePath
    {
        get => (string) GetValue(ImagePathProperty);
        set => SetValue(ImagePathProperty, value);
    }

    public Stretch Stretch
    {
        get => (Stretch) GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    public override XmlElement WriteAttributes(XmlElement element)
    {
        element = base.WriteAttributes(element);
        element.SetAttribute("Type", "ImageTile");
        element.SetAttribute("ImagePath", ImagePath);
        element.SetAttribute("Stretch", Stretch.ToString( ));
        return element;
    }

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        ImagePath = node.GetAttribute("ImagePath");
        Stretch = Enum.Parse<Stretch>(node.GetAttribute("Stretch"));
    }
}
