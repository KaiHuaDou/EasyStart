using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;

namespace StartPro
{
    public class TilesConfig
    {
        private static XmlDocument document = new XmlDocument( );

        static TilesConfig( )
        {
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");
            document.AppendChild(declaration);
        }

        public static void Add(Tile tile)
        {
            XmlElement element = document.CreateElement(tile.AppName);
            element.SetAttribute("AppPath", tile.AppPath);
            element.SetAttribute("AppIcon", tile.AppIcon);
            element.SetAttribute("TileSize", tile.TileSize.ToString( ));
            element.SetAttribute("TileColor", tile.TileColor.ToString( ));
            element.SetAttribute("TileFontSize", tile.TileFontSize.ToString( ));
            document.AppendChild(element);
        }

        public static void Save( )
        {
            File.WriteAllText("config.xml", document.OuterXml);
        }

        public static HashSet<Tile> Load( )
        {
            document.Load("config.xml");
            HashSet<Tile> result = new HashSet<Tile>( );
            foreach (XmlElement element in document.ChildNodes)
            {
                result.Add(new Tile
                {
                    AppPath = element.GetAttribute("AppPath"),
                    AppName = element.Name,
                    AppIcon = element.GetAttribute("AppIcon"),
                    TileSize = (TileType) int.Parse(element.GetAttribute("TileSize")),
                    TileColor = new BrushConverter( ).ConvertFrom(element.GetAttribute("TileColor")) as SolidColorBrush,
                    TileFontSize = double.Parse(element.GetAttribute("TileFontSize"))
                });
            }
            return result;
        }
    }
}