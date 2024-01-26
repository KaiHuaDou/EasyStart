namespace StartPro;

public enum TileType
{
    Small, Medium, Wide, Large
}

public struct TileGrid
{
    public int Row { get; set; }
    public int Col { get; set; }
}

public partial class Tile
{
    public TileGrid GetSize( ) => GetSize(TileSize);

    public static TileGrid GetSize(TileType type)
    {
        return type switch
        {
            TileType.Small => new TileGrid { Row = 1, Col = 1 },
            TileType.Medium => new TileGrid { Row = Defaults.Zoom, Col = Defaults.Zoom },
            TileType.Wide => new TileGrid { Row = Defaults.Zoom, Col = Defaults.Zoom * Defaults.Zoom },
            TileType.Large => new TileGrid { Row = Defaults.Zoom * Defaults.Zoom, Col = Defaults.Zoom * Defaults.Zoom },
            _ => new TileGrid { Row = 1, Col = 1 },
        };
    }
}
