using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml;

namespace StartPro.Tile;


public partial class TileBase
{
    public virtual void WriteAttributes(ref XmlElement element)
    {
        element.SetAttribute("Size", ((int) TileSize).ToString( ));
        element.SetAttribute("Color", TileColor.ToString( ));
        element.SetAttribute("Row", Row.ToString( ));
        element.SetAttribute("Column", Column.ToString( ));
    }

    public virtual void ReadAttributes(XmlNode node)
    {
        TileSize = (TileType) int.Parse(TileConfig.GetAttribute(node, "Size"));
        TileColor = new BrushConverter( ).ConvertFrom(TileConfig.GetAttribute(node, "Color")) as SolidColorBrush;
        FontSize = double.Parse(TileConfig.GetAttribute(node, "FontSize"));
        Row = int.Parse(TileConfig.GetAttribute(node, "Row"));
        Column = int.Parse(TileConfig.GetAttribute(node, "Column"));
    }
}

public partial class AppTile
{
    public override void WriteAttributes(ref XmlElement element)
    {
        base.WriteAttributes(ref element);
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
        AppPath = TileConfig.GetAttribute(node, "Path");
        AppName = TileConfig.GetAttribute(node, "Name");
        AppIcon = TileConfig.GetAttribute(node, "Icon");
        Shadow = bool.Parse(TileConfig.GetAttribute(node, "Shadow"));
        ImageShadow = bool.Parse(TileConfig.GetAttribute(node, "ImageShadow"));
    }
}

public static class TileConfig
{
    private const string xmlPath = "tiles.xml";
    private static readonly XmlDocument document = new( );
    private static XmlNode Apps = document.CreateElement("Tiles");

    public static void Add(TileBase tileBase)
    {
        XmlElement element = document.CreateElement("Tile");
        tileBase.WriteAttributes(ref element);
        Apps.AppendChild(element);
    }

    public static void Save( )
        => File.WriteAllText(xmlPath, Apps.OuterXml);

    public static string GetAttribute(XmlNode node, string name)
        => (node.Attributes.GetNamedItem(name) as XmlAttribute).Value;

    public static HashSet<TileBase> Load( )
    {
        HashSet<TileBase> result = [];
        try { document.Load(xmlPath); } catch { return result; }
        Apps = document.ChildNodes[0];
        foreach (XmlNode node in Apps.ChildNodes)
        {
            AppTile item = new( );
            item.ReadAttributes(node);
            result.Add(item);
        }
        Apps = document.CreateElement("Tiles");
        return result;
    }
}
