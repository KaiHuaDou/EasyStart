namespace StartPro.Api;

public enum UIThemes
{
    AeroNormalColor = 0, Aero2NormalColor = 1,
    LunaNormalColor = 2, LunaHomestead = 3, LunaMetallic = 4, RoyaleNormalColor = 5,
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
