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
        try
        {
            Content = new XmlSerializer(typeof(T)).Deserialize(Reader) as T;
        }
        catch
        {
            Content = new T( );
        }
        Reader.Close( );
        Stream.Close( );
    }

    public void Write( )
    {
        FileStream Stream = new(File, FileMode.Create, FileAccess.Write);
        new XmlSerializer(typeof(T)).Serialize(Stream, Content);
        Stream.Close( );
    }
}
