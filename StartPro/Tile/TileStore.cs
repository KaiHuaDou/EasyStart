using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml;

namespace StartPro.Tile;

public partial class TileBase
{
    public virtual void WriteAttributes(ref XmlElement element)
    {
        element.SetAttribute("Type", "TileBase");
        element.SetAttribute("Size", ((int) TileSize).ToString( ));
        element.SetAttribute("Color", TileColor.ToString( ));
        element.SetAttribute("Row", Row.ToString( ));
        element.SetAttribute("Column", Column.ToString( ));
    }

    public virtual void ReadAttributes(XmlNode node)
    {
        TileSize = (TileType) int.Parse(node.GetAttribute("Size"));
        TileColor = new BrushConverter( ).ConvertFrom(node.GetAttribute("Color")) as SolidColorBrush;
        FontSize = double.Parse(node.GetAttribute("FontSize"));
        Row = int.Parse(node.GetAttribute("Row"));
        Column = int.Parse(node.GetAttribute("Column"));
    }
}

public partial class AppTile
{
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

public static class TileStore
{
    private const string xmlPath = "tiles.xml";
    private static readonly XmlDocument document = new( );
    private static XmlNode Apps = document.CreateElement("Tiles");

    public static void Save(HashSet<TileBase> tiles)
    {
        foreach (TileBase tile in tiles)
        {
            XmlElement element = document.CreateElement("Tile");
            tile.WriteAttributes(ref element);
            Apps.AppendChild(element);
        }
        File.WriteAllText(xmlPath, Apps.OuterXml);
    }

    public static HashSet<TileBase> Load( )
    {
        HashSet<TileBase> result = [];
        try { document.Load(xmlPath); }
        catch { return result; }

        Apps = document.ChildNodes[0];
        foreach (XmlNode node in Apps.ChildNodes)
        {
            try
            {
                TileBase item = node.GetAttribute("Type") switch
                {
                    "AppTile" => new AppTile( ),
                    _ => new TileBase( ),
                };
                item.ReadAttributes(node);
                item.IsEnabled = true;
                result.Add(item);
            }
            catch { continue; }
        }
        Apps = document.CreateElement("Tiles");
        return result;
    }
}

public static class XmlNodeExtend
{
    public static string GetAttribute(this XmlNode node, string name)
        => (node.Attributes.GetNamedItem(name) as XmlAttribute).Value;
}
