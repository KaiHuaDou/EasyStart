using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro
{

    public struct BoardGrid
    {
        public int Row;
        public int Col;
    }
    public partial class MainWindow : Window
    {
        bool[,] BoardPos = new bool[64, 64];

        public MainWindow( )
        {
            InitializeComponent( );
        }

        private void ChangeBoardType(object o, SelectionChangedEventArgs e)
        {
            foreach (Board clip in mainGrid.Children)
            {
                clip.BoardSize = (BoardType) clipTypeCombo.SelectedIndex;
                Grid.SetRowSpan(clip, clipTypeCombo.SelectedIndex + 1);
                Grid.SetColumnSpan(clip, clipTypeCombo.SelectedIndex + 1);
            }
        }

        private void SetGrid(object o, SizeChangedEventArgs e)
        {
            int HeightCnt = (int) Math.Floor(this.ActualHeight / Default.SmallSize);
            int WidthCnt = (int) Math.Floor(this.ActualWidth / Default.SmallSize);
            while (true)
            {
                if (mainGrid.RowDefinitions.Count == HeightCnt)
                    break;
                else if (mainGrid.RowDefinitions.Count < HeightCnt)
                    mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Default.SmallSize + Default.Margin) });
                else
                    mainGrid.RowDefinitions.RemoveAt(0);
            }
            while (true)
            {
                if (mainGrid.ColumnDefinitions.Count == WidthCnt)
                    break;
                else if (mainGrid.ColumnDefinitions.Count < WidthCnt)
                    mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Default.SmallSize + Default.Margin) });
                else
                    mainGrid.ColumnDefinitions.RemoveAt(0);
            }
        }

        Point dragPos;
        Thickness dragMargin;

        private void BoardDragStart(object o, MouseButtonEventArgs e)
        {
            Board c = o as Board;
            BoardGrid pos = PtrPos;
            MarkBoard(pos, c.BoardSize, false);
            Board.IsDrag = true;
            dragPos = e.GetPosition(this);
            dragMargin = c.Margin;
            c.CaptureMouse( );
        }
        private void BoardDragging(object o, MouseEventArgs e)
        {
            if (!Board.IsDrag)
                return;
            var pos = e.GetPosition(this);
            var dp = pos - dragPos;
            Board c = o as Board;
            c.Margin = new Thickness(dragMargin.Left + dp.X, dragMargin.Top + dp.Y, dragMargin.Right - dp.X, dragMargin.Bottom - dp.Y);
        }
        private void BoardDragStop(object o, MouseButtonEventArgs e)
        {
            Board.IsDrag = false;
            Board c = o as Board;
            c.Margin = new Thickness(0);
            c.ReleaseMouseCapture( );
            BoardGrid pos = PtrPos;
            while (!CheckBoard(pos, c.BoardSize))
                pos.Row += 1;
            Grid.SetRow(c, pos.Row);
            Grid.SetColumn(c, pos.Col);
            MarkBoard(pos, c.BoardSize, true);
        }
        private BoardGrid PtrPos
        {
            get
            {
                Point point = Mouse.GetPosition(mainGrid);
                int cellSize = Default.SmallSize + Default.Margin;
                return new BoardGrid
                {
                    Row = (int) Math.Floor(point.Y / cellSize),
                    Col = (int) Math.Floor(point.X / cellSize)
                };
            }
        }

        private void MarkBoard(BoardGrid pos, BoardType type, bool mark = true)
        {
            BoardGrid size = Board.GetSize(type);
            for (int i = 0; i < size.Row; i++)
                for (int j = 0; j < size.Col; j++)
                    BoardPos[pos.Row + i, pos.Col + j] = mark;
        }
        private bool CheckBoard(BoardGrid pos, BoardType type)
        {
            BoardGrid size = Board.GetSize(type);
            for (int i = 0; i < size.Row; i++)
                for (int j = 0; j < size.Col; j++)
                    if (BoardPos[pos.Row + i, pos.Col + j])
                        return false;
            return true;
        }
    }
}
