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
    [DefaultValue("#020202")]
    public string Background { get; set; }

    [DefaultValue((int) UIThemes.AeroNormalColor)]
    public int UITheme { get; set; }
}
