namespace StartPro
{
    public enum BoardType
    {
        Small, Medium, Wide, Large
    }

    public partial class Board
    {
        public static BoardGrid GetSize(BoardType type)
        {
            switch (type)
            {
                case BoardType.Small:
                    return new BoardGrid { Row = 1, Col = 1 };
                case BoardType.Medium:
                    return new BoardGrid { Row = Default.Zoom, Col = Default.Zoom };
                case BoardType.Wide:
                    return new BoardGrid { Row = Default.Zoom, Col = Default.Zoom * Default.Zoom };
                case BoardType.Large:
                    return new BoardGrid { Row = Default.Zoom * Default.Zoom, Col = Default.Zoom * Default.Zoom };
                default:
                    return new BoardGrid { Row = 1, Col = 1 };
            }
        }
    }
}
