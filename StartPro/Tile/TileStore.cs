using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml;

namespace StartPro.Tile;

public partial class TileBase
{
    public virtual XmlElement WriteAttributes(XmlElement element)
    {
        element.SetAttribute("Type", "TileBase");
        element.SetAttribute("Size", ((int) TileSize).ToString( ));
        element.SetAttribute("Color", TileColor.ToString( ));
        element.SetAttribute("Row", Row.ToString( ));
        element.SetAttribute("Column", Column.ToString( ));
        return element;
    }

    public virtual void ReadAttributes(XmlNode node)
    {
        TileSize = (TileSize) int.Parse(node.GetAttribute("Size"));
        TileColor = new BrushConverter( ).ConvertFrom(node.GetAttribute("Color")) as SolidColorBrush;
        FontSize = double.Parse(node.GetAttribute("FontSize"));
        Row = int.Parse(node.GetAttribute("Row"));
        Column = int.Parse(node.GetAttribute("Column"));
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
            element = tile.WriteAttributes(element);
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
                    "TextTile" => new TextTile( ),
                    _ => new TileBase( ),
                };
                item.ReadAttributes(node);
                item.IsEnabled = true;
                result.Add(item);
            }
            catch { }
        }
        Apps = document.CreateElement("Tiles");
        return result;
    }
}

public static class XmlNodeExtend
{
    public static string GetAttribute(this XmlNode node, string name)
        => (node.Attributes.GetNamedItem(name) as XmlAttribute)?.Value;
}
