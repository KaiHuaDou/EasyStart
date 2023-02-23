using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace StartPro
{
    public partial class Tile : UserControl
    {
        private ProcessStartInfo exec = new ProcessStartInfo(Default.AppName);

        public Tile( )
        {
            InitializeComponent( );
            border.DataContext = this;
            TileGrid grid = GetSize( );
            GetSize( ).SetRowSpan(this, grid.Row);
            GetSize( ).SetColumnSpan(this, grid.Col);
        }

        public static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Tile tile = o as Tile;
            string newValue = e.NewValue as string;
            FileInfo app = new FileInfo(newValue);
            tile.exec = new ProcessStartInfo(newValue);
            tile.AppName = app.Name.Replace(app.Extension, "");
            tile.AppIcon = newValue;

        }

        private static void AppIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Tile tile = o as Tile;
            string newValue = e.NewValue as string;
            if (tile.AppIcon == tile.AppName)
            {
                tile.image.Source = StdApi.GetIcon(newValue);
                return;
            }
            try
            {
                tile.image.Source = new BitmapImage(new Uri(newValue));
                return;
            }
            catch { tile.image.Source = StdApi.GetIcon(newValue); }
        }


        private void Execute(object o, MouseButtonEventArgs e)
        {
            if (!IsDrag && IsEnabled)
                Process.Start(exec);
        }
    }
}
