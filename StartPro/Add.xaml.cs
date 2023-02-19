using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace StartPro
{
    public partial class Add : Window
    {
        public Add( ) => InitializeComponent( );

        private void TileSizeChanged(object o, SelectionChangedEventArgs e) 
            => tile.TileSize = (TileType) sizeBox.SelectedIndex;

        private void SelectExe(object o, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                DefaultExt = ".exe",
                Filter = "可执行文件|*.exe;*.com|命令脚本|*.bat;*.cmd|所有文件|*.*",
                Title = "选择程序"
            };
            if (dialog.ShowDialog( ) == true)
            {
                pathBox.Text = dialog.FileName;
                PathChanged(o, e);
            }
        }

        private void SelectIcon(object o, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|所有文件|*.*",
                Title = "选择图标"
            };
            if (dialog.ShowDialog( ) == true)
                iconBox.Text = dialog.FileName;
        }

        private void PathChanged(object o, RoutedEventArgs e)
        {
            try
            {
                tile.AppPath = pathBox.Text;
                nameBox.Text = tile.AppName;
                iconBox.Text = "";
                OkButton.IsEnabled = true;
            }
            catch { OkButton.IsEnabled = false; }
        }


        private void IconChanged(object o, TextChangedEventArgs e)
        {
            try
            {
                tile.AppIcon = new BitmapImage(new Uri(iconBox.Text));
            }
            catch { }
        }

        private void NameChanged(object o, TextChangedEventArgs e)
            => tile.AppName = nameBox.Text;

        private void fontBox_TextChanged(object o, TextChangedEventArgs e)
        {
            try
            {
                tile.TileFontSize = int.Parse(fontBox.Text);
            }
            catch { }
        }

        private void TaskCancel(object o, RoutedEventArgs e)
            => this.Close( );

        private void TaskOk(object sender, RoutedEventArgs e)
        {
            tile.IsEnabled = true;
            mainPanel.Children.Remove(tile);
            this.Close( );
        }
    }
}
