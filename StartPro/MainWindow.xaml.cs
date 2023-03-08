using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace StartPro
{

    public partial class MainWindow : Window
    {
        public MainWindow( )
        {
            InitializeComponent( );
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); ;
            HashSet<Tile> tiles = Tile.Load( );
            foreach (Tile tile in tiles)
            {
                tile.Init( );
                mainGrid.Children.Add(tile);
            }
        }

        private void SetGrid(object o, SizeChangedEventArgs e)
        {
            int HeightCnt = (int) Math.Floor(this.ActualHeight / Default.SmallSize);
            int WidthCnt = (int) Math.Floor(this.ActualWidth / Default.SmallSize);
            while (true)
            {
                if (mainGrid.RowDefinitions.Count == HeightCnt) break;
                else if (mainGrid.RowDefinitions.Count < HeightCnt)
                    mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(Default.SmallSize + Default.Margin) });
                else mainGrid.RowDefinitions.RemoveAt(0);
            }
            while (true)
            {
                if (mainGrid.ColumnDefinitions.Count == WidthCnt) break;
                else if (mainGrid.ColumnDefinitions.Count < WidthCnt)
                    mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(Default.SmallSize + Default.Margin) });
                else mainGrid.ColumnDefinitions.RemoveAt(0);
            }
        }

        private void AddTile(object sender, RoutedEventArgs e)
        {
            Creat window = new Creat( );
            window.ShowDialog( );
            Tile tile = window.tile;
            if (!tile.IsEnabled) return;
            mainGrid.Children.Add(tile);
        }

        private void SaveTiles(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Tile tile in mainGrid.Children)
                Tile.Add(tile);
            Tile.Save( );
        }
    }
}
