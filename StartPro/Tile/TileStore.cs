using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using StartPro.Api;

namespace StartPro.Tile;

public static class TileStore
{
    private static readonly string xmlPath = Path.Join(Utils.ParentDir, "tiles.xml");
    private static readonly XmlDocument document = new( );
    private static XmlNode Tiles;

    public static List<TileBase> Load( )
    {
        List<TileBase> result = [];
        try
        {
            document.Load(xmlPath);
        }
        catch
        {
            if (File.Exists(xmlPath))
            {
                File.Copy(xmlPath, "tiles.bak", true);
                App.AddInfo("无法读取磁贴配置文件，旧文件已备份");
            }
            else
            {
                File.Create(xmlPath).Close( );
                App.AddInfo("配置文件不存在，已创建新文件");
            }
            return result;
        }

        Tiles = document.ChildNodes[0];
        if (Tiles is null)
            return [];

        foreach (XmlNode node in Tiles.ChildNodes)
        {
            if (node.Name != "Tile")
                continue;
            try
            {
                TileBase item = node.GetAttribute("Type") switch
                {
                    "AppTile" => new AppTile( ),
                    "TextTile" => new TextTile( ),
                    "ImageTile" => new ImageTile( ),
                    "__MFGM__" => new DebugTile( ),
                    _ => new TileBase( ),
                };
                item.ReadAttributes(node);
                item.IsEnabled = true;
                result.Add(item);
            }
            catch
            {
                App.AddInfo("存在无法读取的磁贴，已跳过");
            }
        }
        return result;
    }

    public static bool Save( )
    {
        Tiles = document.CreateElement("Tiles");
        foreach (TileBase tile in App.Tiles)
        {
            XmlElement element = document.CreateElement("Tile");
            tile.WriteAttributes(ref element);
            Tiles.AppendChild(element);
        }
        try
        {
            File.WriteAllText(xmlPath, Tiles.OuterXml);
        }
        catch (Exception ex)
        {
            App.AddInfo($"无法保存磁贴: {ex.Message}");
            return false;
        }
        return true;
    }
}

