using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml;

namespace StartPro.Tile;

public static class TileStore
{
    private const string xmlPath = "tiles.xml";
    private static readonly XmlDocument document = new( );
    private static XmlNode Apps = document.CreateElement("Tiles");

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
            catch { throw; }
        }
        Apps = document.CreateElement("Tiles");
        return result;
    }

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
}

public static class XmlNodeExtend
{
    public static string GetAttribute(this XmlNode node, string name)
        => (node.Attributes.GetNamedItem(name) as XmlAttribute)?.Value;
}
