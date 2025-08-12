using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace StartPro.Tile;

public static class TileStore
{
    private const string xmlPath = "tiles.xml";
    private static readonly XmlDocument document = new( );
    private static XmlNode Apps;

    public static ObservableCollection<TileBase> Load( )
    {
        ObservableCollection<TileBase> result = [];
        try
        {
            document.Load(xmlPath);
            File.Copy(xmlPath, "tiles.bak", true);
        }
        catch
        {
            App.ShowInfo("无法读取磁贴配置文件，旧文件已备份");
            return result;
        }

        Apps = document.ChildNodes[0];
        foreach (XmlNode node in Apps.ChildNodes)
        {
            try
            {
                TileBase item = node.GetAttribute("Type") switch
                {
                    "AppTile" => new AppTile( ),
                    "TextTile" => new TextTile( ),
                    "ImageTile" => new ImageTile( ),
                    _ => new TileBase( ),
                };
                item.ReadAttributes(node);
                item.IsEnabled = true;
                result.Add(item);
            }
            catch
            {
                App.ShowInfo("存在无法读取的磁贴，已跳过");
            }
        }
        return result;
    }

    public static void Save( )
    {
        Apps = document.CreateElement("Tiles");
        foreach (TileBase tile in App.Tiles)
        {
            XmlElement element = document.CreateElement("Tile");
            tile.WriteAttributes(ref element);
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
