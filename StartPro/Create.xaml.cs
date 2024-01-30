using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace StartPro;

public partial class Create : Window
{
    public Tile tile = new( );

    public Create(Tile t = null)
    {
        InitializeComponent( );
        if (t is not null)
        {
            tile = t;
            iconBox.Text = tile.AppIcon /*= t.AppIcon*/;
            nameBox.Text = tile.AppName /*= t.AppName*/;
            pathBox.Text = tile.AppPath /*= t.AppPath*/;

            // 更改 IsChecked 的同时会触发 Checked 或 Unchecked
            ShadowBox.IsChecked = tile.Shadow;
            ImageShadowBox.IsChecked = tile.ImageShadow;

            //tile.TileSize = t.TileSize;
            //tile.TileColor = t.TileColor;
            //tile.Shadow = t.Shadow;
            //tile.ImageShadow = t.ImageShadow;

            sizeBox.SelectedIndex = (int) tile.TileSize;
            OkButton.IsEnabled = true;
            mainPanel.Children.Add(tile);
        }
        else
        {
            tile = new Tile( );
            tile.Row = tile.Column = 0;
        }
        DockPanel.SetDock(tile, Dock.Right);
        tile.Margin = new Thickness(5, 10, 10, 5);
        tile.IsEnabled = false;
    }

    private void TileSizeChanged(object o, SelectionChangedEventArgs e)
        => tile.TileSize = (TileType) sizeBox.SelectedIndex;

    private void SelectExe(object o, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new( )
        {
            CheckFileExists = true,
            DefaultExt = ".exe",
            Filter = "*.exe *.com *.bat *.cmd|*.exe;*.com;*.bat;*.cmd|*.*|*.*",
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
        OpenFileDialog dialog = new( )
        {
            CheckFileExists = true,
            Filter = "*.jpg *.jpeg *.png *.bmp *.tif *.tiff *.gif *.ico|*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.tiff;*.gif;*.ico|*.*|*.*",
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
            iconBox.Text = tile.AppIcon;
            OkButton.IsEnabled = true;
        }
        catch { OkButton.IsEnabled = false; }
    }

    private void IconChanged(object o, TextChangedEventArgs e)
    {
        try { tile.AppIcon = new Uri(iconBox.Text).LocalPath; } catch { }
    }

    private void NameChanged(object o, TextChangedEventArgs e)
        => tile.AppName = nameBox.Text;

    private void FontChanged(object o, TextChangedEventArgs e)
        => tile.FontSize = double.TryParse(fontBox.Text, out double result) ? result : Defaults.FontSize;

    private void SelectColor(object o, RoutedEventArgs e)
    {
        System.Windows.Forms.ColorDialog dialog = new( )
        {
            AllowFullOpen = true,
            AnyColor = true,
        };
        if (dialog.ShowDialog( ) == System.Windows.Forms.DialogResult.OK)
        {
            tile.TileColor = new SolidColorBrush(
                Color.FromArgb(dialog.Color.A, dialog.Color.R,
                               dialog.Color.G, dialog.Color.B));
        }
    }

    private void ShadowBoxChecked(object o, RoutedEventArgs e)
        => tile.Shadow = !tile.Shadow;

    private void ImageShadowBoxChecked(object o, RoutedEventArgs e)
        => tile.ImageShadow = !tile.ImageShadow;

    private void TaskCancel(object o, RoutedEventArgs e)
        => Close( );

    private void TaskOk(object o, RoutedEventArgs e)
    {
        mainPanel.Children.Remove(tile);
        tile.IsEnabled = true;
        tile.Margin = new Thickness(Defaults.Margin);
        Close( );
    }
}
