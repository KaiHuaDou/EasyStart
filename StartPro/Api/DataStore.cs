using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace StartPro.Api;

public class DataStore<T> where T : class, new()
{
    public T Content { get; set; }

    private readonly string File;

    public DataStore(string xmlFile)
    {
        File = new FileInfo(xmlFile).FullName;
        try
        {
            Read( );
        }
        catch (IOException) { }
    }

    public void Read( )
    {
        FileStream Stream = new(File, FileMode.OpenOrCreate, FileAccess.Read);
        XmlReader Reader = XmlReader.Create(Stream);
        XmlSerializer serializer = new(typeof(T));
        try
        {
            Content = serializer.Deserialize(Reader) as T;
        }
        catch
        {
            Content = new T( );
        }
    }

    public void Save( )
    {
        FileStream Stream = new(File, FileMode.OpenOrCreate, FileAccess.Write);
        new XmlSerializer(typeof(T)).Serialize(Stream, Content);
    }
}
