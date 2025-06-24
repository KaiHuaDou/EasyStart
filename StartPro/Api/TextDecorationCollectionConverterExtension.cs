using System.Collections.Generic;
using System.Windows;

namespace StartPro.Api;
public static class TextDecorationCollectionConverterExtension
{
    public static string ConvertToString(this TextDecorationCollection collection)
    {
        if (collection == null || collection.Count == 0)
            return "None";

        Dictionary<TextDecoration, string> decorationMap = new( )
        {
            { TextDecorations.OverLine[0], "OverLine" },
            { TextDecorations.Baseline[0], "BaseLine" },
            { TextDecorations.Underline[0], "UnderLine" },
            { TextDecorations.Strikethrough[0], "StrikeThrough" }
        };

        List<string> decorations = [];

        foreach (KeyValuePair<TextDecoration, string> entry in decorationMap)
        {
            if (collection.Contains(entry.Key))
                decorations.Add(entry.Value);
        }

        return string.Join(", ", decorations);
    }
}
