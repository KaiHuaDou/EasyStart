using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace StartPro.Api;

public class DataStore<T> : IDisposable where T : class, new()
{
    public T Content { get; set; }

    private readonly string File;
    private readonly FileStream Stream;
    private readonly XmlReader Reader;

    public DataStore(string xmlFile)
    {
        File = new FileInfo(xmlFile).FullName;
        Stream = new(File, FileMode.OpenOrCreate);
        Reader = XmlReader.Create(Stream);
        Read( );
    }

    public void Read( )
    {
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
        Stream.SetLength(0);
        XmlSerializer serializer = new(typeof(T));
        serializer.Serialize(Stream, Content);
    }

    public void Dispose( )
    {
        Reader.Dispose( );
        Stream.Dispose( );
        GC.SuppressFinalize(this);
    }
}
