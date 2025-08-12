using System.IO;
using System.Text.Json;

namespace StartPro.Api;

public enum UIThemes
{
    AeroNormalColor = 0,
    Aero2NormalColor = 1,
    LunaNormalColor = 2,
    LunaHomestead = 3,
    LunaMetallic = 4,
    RoyaleNormalColor = 5,
    Classic = 6
}

public class Config
{
    public string Background
    {
        get;
        set => field = string.IsNullOrWhiteSpace(value) ? "#FFEEEEEE" : value;
    } = "#FFEEEEEE";

    public int UITheme { get; set; } = (int) UIThemes.AeroNormalColor;

    public bool UIFlat { get; set; }
}

public class ConfigStore<T> where T : class, new()
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

    public ConfigStore(string xmlFile)
    {
        File = new FileInfo(xmlFile);
        Read( );
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
            File.CopyTo(File.FullName + ".bak", true);
            Content = new T( );
            App.ShowInfo("配置文件读取失败，旧配置文件已备份");
        }
    }

    public void Write( )
    {
        using FileStream Stream = new(File.FullName, FileMode.Create, FileAccess.Write);
        JsonSerializer.Serialize(Stream, Content, Options);
    }
}
