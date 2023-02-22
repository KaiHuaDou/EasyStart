using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StartPro
{
    public partial class Tile : UserControl
    {
        private ProcessStartInfo exec = new ProcessStartInfo(Default.AppName);

        public Tile( )
        {
            InitializeComponent( );
            border.DataContext = this;
        }

        public static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            Tile tile = o as Tile;
            tile.exec = new ProcessStartInfo(e.NewValue as string);
            tile.AppName = new FileInfo(e.NewValue as string).Name;
            tile.AppIcon = e.NewValue as string;

        }

        private static void AppIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            (o as Tile).image.Source = StdApi.GetIcon(e.NewValue as string);
        }


        private void Execute(object o, MouseButtonEventArgs e)
        {
            if (!IsDrag && IsEnabled)
                Process.Start(exec);
        }
    }
}
