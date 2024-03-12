using System.ComponentModel;

namespace StartPro.Api;

public enum UIThemes
{
    AeroNormalColor = 0, Aero2NormalColor,
    LunaNormalColor, LunaHomestead, LunaMetallic, RoyaleNormalColor,
    Classic
}

public class Config
{
    public const string backgroundDefault = "#EEEEEEE";
    private string background;

    [DefaultValue(backgroundDefault)]
    public string Background
    {
        get => background;
        set => background = string.IsNullOrWhiteSpace(value) ? background : value;
    }

    [DefaultValue((int) UIThemes.AeroNormalColor)]
    public int UITheme { get; set; }

    [DefaultValue(false)]
    public bool UIFlat { get; set; }
}
