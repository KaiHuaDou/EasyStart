using System.Collections.Generic;
using System.Windows;

namespace StartPro.Tile;

public enum TileSize
{
    Small, Medium, Wide, Thin, High, Tall, Large
}

public static class TileDatas
{
    public static int BaseSize => 64;
    public static int BaseMargin => 10; 
    public static int BaseRadius => App.Program.Settings.Content.UIFlat ? 0 : 10;

    public static int BlockSize => BaseSize + BaseMargin;

    public static Dictionary<TileSize, (int, int)> TileSizes { get; } = new( )
    {
        {TileSize.Small,  (1 * BaseSize + 0 * BaseMargin, 1 * BaseSize + 0 * BaseMargin)},
        {TileSize.Medium, (2 * BaseSize + 1 * BaseMargin, 2 * BaseSize + BaseMargin)},
        {TileSize.Thin,   (4 * BaseSize + 3 * BaseMargin, 1 * BaseSize + 0 * BaseMargin)},
        {TileSize.Wide,   (4 * BaseSize + 3 * BaseMargin, 2 * BaseSize + BaseMargin)},
        {TileSize.Tall,   (1 * BaseSize + 0 * BaseMargin, 4 * BaseSize + 3 * BaseMargin)},
        {TileSize.High,   (2 * BaseSize + 1 * BaseMargin, 4 * BaseSize + 3 * BaseMargin)},
        {TileSize.Large,  (4 * BaseSize + 3 * BaseMargin, 4 * BaseSize + 3 * BaseMargin) },
    };

    public static Dictionary<TileSize, int> TileRadius { get; } = new( )
    {
        {TileSize.Small  , BaseRadius / 2 },
        {TileSize.Medium , BaseRadius },
        {TileSize.Thin   , BaseRadius },
        {TileSize.Wide   , BaseRadius },
        {TileSize.Tall   , BaseRadius },
        {TileSize.High   , BaseRadius },
        {TileSize.Large  , BaseRadius * 2},
    };
}
