using System.Windows.Media;
using System.Xml;

namespace StartPro.Tile;

public interface IStorable
{
    void ReadAttributes(XmlNode node);
    XmlElement WriteAttributes(XmlElement element);
}

public partial class TileBase : IStorable
{
    public virtual void ReadAttributes(XmlNode node)
    {
        TileSize = (TileSize) int.Parse(node.GetAttribute("Size"));
        TileColor = new BrushConverter( ).ConvertFrom(node.GetAttribute("Color")) as SolidColorBrush;
        Row = int.Parse(node.GetAttribute("Row"));
        Column = int.Parse(node.GetAttribute("Column"));
    }

    public virtual XmlElement WriteAttributes(XmlElement element)
    {
        element.SetAttribute("Size", ((int) TileSize).ToString( ));
        element.SetAttribute("Color", TileColor.ToString( ));
        element.SetAttribute("Row", Row.ToString( ));
        element.SetAttribute("Column", Column.ToString( ));
        return element;
    }
}

public partial class TileBase
{
    public static T Clone<T>(T tile) where T : TileBase, new()
    {
        XmlDocument doc = new( );
        XmlElement root = doc.CreateElement("Tile");
        doc.AppendChild(root);
        tile.WriteAttributes(root);
        T result = new( );
        result.ReadAttributes(doc.DocumentElement);
        return result;
    }
}
