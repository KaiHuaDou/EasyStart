using System.IO;
using System.Text.Json;

namespace StartPro.Api;

public class DataStore<T> where T : class, new()
{
    public T Content { get; set; }

    private readonly FileInfo File;

    private readonly JsonSerializerOptions Options = new( )
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        WriteIndented = true
    };

    public DataStore(string xmlFile)
    {
        File = new FileInfo(xmlFile);
        try
        {
            Read( );
        }
        catch (IOException) { }
    }

    public void Read( )
    {
        if (!File.Exists || File.Length == 0)
        {
            Content = new T( );
            return;
        }
        using FileStream Stream = new(File.FullName, FileMode.OpenOrCreate, FileAccess.Read);
        try
        {
            Content = JsonSerializer.Deserialize<T>(Stream, Options) ?? new T( );
        }
        catch
        {
            Content = new T( );
        }
    }

    public void Write( )
    {
        using FileStream Stream = new(File.FullName, FileMode.Create, FileAccess.Write);
        JsonSerializer.Serialize(Stream, Content, Options);
    }
}
