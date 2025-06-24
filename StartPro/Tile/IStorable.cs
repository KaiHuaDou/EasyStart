using System.Xml;

namespace StartPro.Tile;

public interface IStorable
{
    public void ReadAttributes(XmlNode node);
    public XmlElement WriteAttributes(XmlElement element);

}
