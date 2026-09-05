using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Xml;

using StartPro.Api;

namespace StartPro.Tile;

public interface IStorable
{
    void ReadAttributes(XmlNode node);
    void WriteAttributes(ref XmlElement element);
}

public partial class TileBase : IStorable
{
    public virtual void ReadAttributes(XmlNode node)
    {
        TileSize = node.FromAttribute("Size", TileSize.Medium);
        TileColor = new SolidColorBrush(node.FromAttribute("Color", Defaults.TileColorBrush.Color));
        Row = node.FromAttribute("Row", 0);
        Column = node.FromAttribute("Column", 0);
        Shadow = node.FromAttribute("Shadow", false);
    }

    public virtual void WriteAttributes(ref XmlElement element)
    {
        element.SetAttribute("Size", ((int) TileSize).ToString( ));
        element.SetAttribute("Color", TileColor.ToString( ));
        element.SetAttribute("Row", Row.ToString( ));
        element.SetAttribute("Column", Column.ToString( ));
        element.SetAttribute("Shadow", Shadow.ToString( ));
    }
}

public partial class TileBase
{
    public static T Clone<T>(T tile) where T : TileBase, new()
    {
        XmlDocument doc = new( );
        var root = doc.CreateElement("Tile");
        doc.AppendChild(root);
        tile.WriteAttributes(ref root);
        T result = new( );
        result.ReadAttributes(doc.DocumentElement!);
        return result;
    }
}

public static class XmlNodeExtend
{
    public static T FromAttribute<T>(this XmlNode node, string name, T fallback)
    {
        var s = node.GetAttribute(name);
        if (string.IsNullOrWhiteSpace(s) || s == "__DEFAULT__")
        {
            return fallback;
        }

        var type = typeof(T);
        if (type.IsEnum)
        {
            return Enum.TryParse(type, s, true, out var enumVal) ? (T) enumVal : fallback;
        }

        if (type == typeof(bool))
        {
            return bool.TryParse(s, out var boolVal) ? (T) (object) boolVal : fallback;
        }

        if (type == typeof(double))
        {
            return double.TryParse(s, out var doubleVal) ? (T) (object) doubleVal : fallback;
        }

        if (type == typeof(int))
        {
            return int.TryParse(s, out var doubleVal) ? (T) (object) doubleVal : fallback;
        }

        var conv = TypeDescriptor.GetConverter(type);
        try
        {
            var o = conv.ConvertFromString(s);
            return o is T t ? t : fallback;
        }
        catch
        {
            return fallback;
        }
    }

    public static string? GetAttribute(this XmlNode node, string name)
    {
        return (node.Attributes?.GetNamedItem(name) as XmlAttribute)?.Value;
    }
}
