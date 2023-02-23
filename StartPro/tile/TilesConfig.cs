using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace StartPro
{
    public static class TilesConfig
    {
        private static XmlDocument document = new XmlDocument( );

        public static void Add(Tile tile)
        {
            XmlElement element = document.CreateElement(tile.AppName);
            element.SetAttribute("AppPath", tile.AppPath);
            element.SetAttribute("AppIcon", tile.AppIcon);
            element.SetAttribute("TileType", ((int) tile.TileSize).ToString( ));
            element.SetAttribute("TileColor", tile.TileColor.ToString( ));
            element.SetAttribute("TileFontSize", tile.TileFontSize.ToString( ));
            element.SetAttribute("Row", Grid.GetRow(tile).ToString( ));
            element.SetAttribute("Column", Grid.GetColumn(tile).ToString( ));
            document.AppendChild(element);
        }

        public static void Save( )
        {
            File.WriteAllText("config.xml", document.OuterXml);
        }

        private static string GetAttribute(XmlNode node, string name)
            => (node.Attributes.GetNamedItem(name) as XmlAttribute).Value;

        public static HashSet<Tile> Load( )
        {
            HashSet<Tile> result = new HashSet<Tile>( );
            document.Load("config.xml");
            foreach (XmlNode node in document.ChildNodes)
            {
                Tile item = new Tile
                {
                    AppPath = GetAttribute(node, "AppPath"),
                    AppName = node.Name,
                    AppIcon = GetAttribute(node, "AppIcon"),
                    TileSize = (TileType) int.Parse(GetAttribute(node, "TileType")),
                    TileColor = new BrushConverter( ).ConvertFrom(GetAttribute(node, "TileColor")) as SolidColorBrush,
                    TileFontSize = double.Parse(GetAttribute(node, "TileFontSize")),
                }; 
                Grid.SetRow(item, int.Parse(GetAttribute(node, "Row")));
                Grid.SetColumn(item, int.Parse(GetAttribute(node, "Column")));
                result.Add(item);
            }
            document = new XmlDocument( );
            return result;
        }
    }
}