using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Xml;

namespace StartPro;

public static class TileConfig
{
    private const string xmlPath = "tiles.xml";
    private static readonly XmlDocument document = new( );
    private static XmlNode Apps = document.CreateElement("Apps");

    public static void Add(Tile tile)
    {
        XmlElement element = document.CreateElement("App");
        element.SetAttribute("Name", tile.AppName);
        element.SetAttribute("Path", tile.AppPath);
        element.SetAttribute("Icon", tile.AppIcon);
        element.SetAttribute("Size", ((int) tile.TileSize).ToString( ));
        element.SetAttribute("Color", tile.TileColor.ToString( ));
        element.SetAttribute("FontSize", tile.FontSize.ToString( ));
        element.SetAttribute("Row", tile.Row.ToString( ));
        element.SetAttribute("Column", tile.Column.ToString( ));
        Apps.AppendChild(element);
    }

    public static void Save( )
        => File.WriteAllText(xmlPath, Apps.OuterXml);

    private static string GetAttribute(XmlNode node, string name)
        => (node.Attributes.GetNamedItem(name) as XmlAttribute).Value;

    public static HashSet<Tile> Load( )
    {
        HashSet<Tile> result = [];
        try { document.Load(xmlPath); } catch { return result; }
        Apps = document.ChildNodes[0];
        foreach (XmlNode node in Apps.ChildNodes)
        {
            Tile item = new( )
            {
                AppPath = GetAttribute(node, "Path"),
                AppName = GetAttribute(node, "Name"),
                AppIcon = GetAttribute(node, "Icon"),
                TileSize = (TileType) int.Parse(GetAttribute(node, "Size")),
                TileColor = new BrushConverter( ).ConvertFrom(GetAttribute(node, "Color")) as SolidColorBrush,
                FontSize = double.Parse(GetAttribute(node, "FontSize")),
                Row = int.Parse(GetAttribute(node, "Row")),
                Column = int.Parse(GetAttribute(node, "Column")),
            };
            item.Init( );
            result.Add(item);
        }
        Apps = document.CreateElement("Apps");
        return result;
    }
}
