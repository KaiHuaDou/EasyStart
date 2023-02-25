using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace StartPro
{
    public partial class Tile
    {
        private const string cfgPath = "tiles.xml";
        private static XmlDocument document = new XmlDocument( );
        private static XmlNode Apps = document.CreateElement("Apps");

        public static void Add(Tile tile)
        {
            XmlElement element = document.CreateElement("App");
            element.SetAttribute("AppName", tile.AppName);
            element.SetAttribute("AppPath", tile.AppPath);
            element.SetAttribute("AppIcon", tile.AppIcon);
            element.SetAttribute("TileSize", ((int) tile.TileSize).ToString( ));
            element.SetAttribute("TileColor", tile.TileColor.ToString( ));
            element.SetAttribute("TileFontSize", tile.TileFontSize.ToString( ));
            element.SetAttribute("Row", Grid.GetRow(tile).ToString( ));
            element.SetAttribute("Column", Grid.GetColumn(tile).ToString( ));
            Apps.AppendChild(element);
        }

        public static void Save( )
            => File.WriteAllText(cfgPath, Apps.OuterXml);

        private static string GetAttribute(XmlNode node, string name)
            => (node.Attributes.GetNamedItem(name) as XmlAttribute).Value;

        public static HashSet<Tile> Load( )
        {
            HashSet<Tile> result = new HashSet<Tile>( );
            document.Load(cfgPath);
            Apps = document.ChildNodes[0];
            foreach (XmlNode node in Apps.ChildNodes)
            {
                Tile item = new Tile
                {
                    AppPath = GetAttribute(node, "AppPath"),
                    AppName = GetAttribute(node, "AppName"),
                    AppIcon = GetAttribute(node, "AppIcon"),
                    TileSize = (TileType) int.Parse(GetAttribute(node, "TileSize")),
                    TileColor = new BrushConverter( ).ConvertFrom(GetAttribute(node, "TileColor")) as SolidColorBrush,
                    TileFontSize = double.Parse(GetAttribute(node, "TileFontSize")),
                };
                Grid.SetRow(item, int.Parse(GetAttribute(node, "Row")));
                Grid.SetColumn(item, int.Parse(GetAttribute(node, "Column")));
                result.Add(item);
            }
            Apps = document.CreateElement("Apps");
            return result;
        }
    }
}