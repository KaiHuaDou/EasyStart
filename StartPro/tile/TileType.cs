namespace StartPro
{
    public enum TileType
    {
        Small, Medium, Wide, Large
    }

    public struct TileGrid
    {
        public int Row;
        public int Col;
    }

    public partial class Tile
    {
        public static TileGrid GetSize(TileType type)
        {
            switch (type)
            {
                case TileType.Small: return new TileGrid { Row = 1, Col = 1 };
                case TileType.Medium: return new TileGrid { Row = Default.Zoom, Col = Default.Zoom };
                case TileType.Wide: return new TileGrid { Row = Default.Zoom, Col = Default.Zoom * Default.Zoom };
                case TileType.Large: return new TileGrid { Row = Default.Zoom * Default.Zoom, Col = Default.Zoom * Default.Zoom };
                default: return new TileGrid { Row = 1, Col = 1 };
            }
        }
    }
}
