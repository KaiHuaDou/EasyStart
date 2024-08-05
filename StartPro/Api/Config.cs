namespace StartPro.Api;

public enum UIThemes
{
    AeroNormalColor = 0, Aero2NormalColor,
    LunaNormalColor, LunaHomestead, LunaMetallic, RoyaleNormalColor,
    Classic
}

public class Config
{
    private string background = "#FFEEEEEE";

    public string Background
    {
        get => background;
        set => background = string.IsNullOrWhiteSpace(value) ? "#FFEEEEEE" : value;
    }

    public int UITheme { get; set; } = (int) UIThemes.AeroNormalColor;

    public bool UIFlat { get; set; }
}
