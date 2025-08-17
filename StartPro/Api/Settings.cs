using System;
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

public class Settings
{
    private const string xml = "settings.json";
    private static readonly FileInfo File = new(xml);
    private static readonly FileStream FileStream = new(File.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    private readonly JsonSerializerOptions Options = new( )
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        WriteIndented = true
    };

    public string Background
    {
        get;
        set => field = string.IsNullOrWhiteSpace(value) ? "#FFFAFAFA" : value;
    } = "#FFFAFAFA";

    public string Foreground
    {
        get;
        set => field = string.IsNullOrWhiteSpace(value) ? "#FF060606" : value;
    } = "#FF060606";

    public bool UIFlat { get; set; }

    public int UITheme { get; set; } = (int) UIThemes.AeroNormalColor;

    public Settings Read( )
    {
        if (!File.Exists || File.Length == 0)
        {
            return new Settings( );
        }
        try
        {
            return JsonSerializer.Deserialize<Settings>(FileStream, Options) ?? new Settings( );
        }
        catch
        {
            File.CopyTo(File.FullName + ".bak", true);
            App.AddInfo("配置文件读取失败，旧配置文件已备份");
            return new Settings( );
        }
    }

    public bool Write( )
    {
        try
        {
            JsonSerializer.Serialize(FileStream, this, Options);
        }
        catch (Exception ex)
        {
            App.AddInfo($"配置文件写入失败: {ex.Message}");
            return false;
        }
        return true;
    }
}
