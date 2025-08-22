using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StartPro.Api;

public enum UIThemes
{
    AeroNormalColor = 0, // Aero 万岁！
    Fluent = 1,
    Aero2NormalColor = 2,
    LunaNormalColor = 3,
    LunaHomestead = 4,
    LunaMetallic = 5,
    RoyaleNormalColor = 6,
    Classic = 7,
}

public static class UIThemesExtensions
{
    private static readonly Dictionary<UIThemes, string> ThemeUris = new( )
    {
        { UIThemes.AeroNormalColor, "pack://application:,,,/PresentationFramework.Aero;component/themes/Aero.NormalColor.xaml" },
        { UIThemes.Fluent, "pack://application:,,,/PresentationFramework.Fluent;component/Themes/Fluent.xaml" },
        { UIThemes.Aero2NormalColor, "pack://application:,,,/PresentationFramework.Aero2;component/themes/Aero2.NormalColor.xaml" },
        { UIThemes.LunaNormalColor, "pack://application:,,,/PresentationFramework.Luna;component/themes/Luna.NormalColor.xaml" },
        { UIThemes.LunaHomestead, "pack://application:,,,/PresentationFramework.Luna;component/themes/Luna.Homestead.xaml" },
        { UIThemes.LunaMetallic, "pack://application:,,,/PresentationFramework.Luna;component/themes/Luna.Metallic.xaml" },
        { UIThemes.RoyaleNormalColor, "pack://application:,,,/PresentationFramework.Luna;component/themes/Royale.NormalColor.xaml" },
        { UIThemes.Classic, "pack://application:,,,/PresentationFramework.Classic;component/themes/Classic.xaml" },
    };

    public static Uri GetUri(this UIThemes theme) => new(ThemeUris[theme]);
}

public class Settings
{
    private static readonly string xml = Path.Join(Utils.ParentDir, "settings.json");
    private static readonly FileInfo File = new(xml);
    private static readonly FileStream FileStream = new(File.FullName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

    public string Background
    {
        get;
        set => field = string.IsNullOrWhiteSpace(value) ? Defaults.BackgroundColorText : value;
    } = Defaults.BackgroundColorText;

    public string Foreground
    {
        get;
        set => field = string.IsNullOrWhiteSpace(value) ? Defaults.ForegroundColorText : value;
    } = Defaults.ForegroundColorText;

    public bool UIFlat { get; set; }

    public int UITheme { get; set; } = (int) UIThemes.AeroNormalColor;

    public static Settings Read( )
    {
        try
        {
            FileStream.Seek(0, SeekOrigin.Begin);
            return JsonSerializer.Deserialize(FileStream, SettingsContext.Default.Settings) ?? new Settings( );
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
            FileStream.Seek(0, SeekOrigin.Begin);
            JsonSerializer.Serialize(FileStream, this, SettingsContext.Default.Settings);
        }
        catch (Exception ex)
        {
            App.AddInfo($"配置文件写入失败: {ex.Message}");
            return false;
        }
        return true;
    }
}

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    AllowTrailingCommas = true,
    WriteIndented = true
)]
[JsonSerializable(typeof(Settings))]
internal sealed partial class SettingsContext : JsonSerializerContext;
