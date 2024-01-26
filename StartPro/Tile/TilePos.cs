namespace StartPro;

public partial class Tile
{
    private static bool[,] TilePos = new bool[64, 64];

    public static void SetTilePos(TileGrid pos, TileType type, bool mark = true)
    {
        TileGrid size = GetSize(type);
        for (int i = 0; i < size.Row; i++)
            for (int j = 0; j < size.Col; j++)
                TilePos[pos.Row + i, pos.Col + j] = mark;
    }

    public static bool IsPosEmpty(TileGrid pos, TileType type)
    {
        TileGrid size = GetSize(type);
        for (int i = 0; i < size.Row; i++)
            for (int j = 0; j < size.Col; j++)
                if (TilePos[pos.Row + i, pos.Col + j])
                    return false;
        return true;
    }
}
