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
        Stream = new(File, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        Reader = XmlReader.Create(Stream);
        try
        {
            Read( );
        }
        catch (IOException) { }
    }

    public void Read( )
    {
        Stream.Seek(0, SeekOrigin.Begin);
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
        Stream.Seek(0, SeekOrigin.Begin);
        new XmlSerializer(typeof(T)).Serialize(Stream, Content);
    }

    public void Dispose( )
    {
        Stream.Dispose( );
        Reader.Dispose( );
        GC.SuppressFinalize(this);
    }
}
