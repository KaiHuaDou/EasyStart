using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StartPro
{

    public partial class MainWindow : Window
    {
        private Point dragPos;
        private Thickness dragMargin;

        public MainWindow( )
        {
            InitializeComponent( );
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

        private void BoardDragStart(object o, MouseButtonEventArgs e)
        {
            Board c = o as Board;
            BoardGrid pos = PtrPos;
            Board.SetBoardPos(pos, c.BoardSize, false);
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
            if (!Board.IsPosEmpty(pos, c.BoardSize))
                return;
            Grid.SetRow(c, pos.Row);
            Grid.SetColumn(c, pos.Col);
            Board.SetBoardPos(pos, c.BoardSize, true);
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

        private void AddBoard(object sender, RoutedEventArgs e)
        {
            Add window = new Add( );
            window.ShowDialog( );
            if (window.board.IsEnabled == true)
            {
                Board board = window.board;
                BoardGrid grid = Board.GetSize(board.BoardSize);
                while (!Board.IsPosEmpty(grid, board.BoardSize))
                    grid.Col += 1;
                Grid.SetRowSpan(board, grid.Row);
                Grid.SetColumnSpan(board, grid.Col);
                board.MouseRightButtonDown += BoardDragStart;
                board.MouseMove += BoardDragging;
                board.MouseRightButtonUp += BoardDragStop;
                board.Margin = new Thickness(0);
                mainGrid.Children.Add(board);
            }
        }
    }
}
