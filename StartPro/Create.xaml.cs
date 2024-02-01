using System;
using System.ComponentModel;
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
            tile.IsEnabled = true;
            OkButton.IsEnabled = true;

            iconBox.Text = tile.AppIcon;
            nameBox.Text = tile.AppName;
            pathBox.Text = tile.AppPath;
        }
        else
        {
            tile = new Tile
            {
                IsEnabled = false,
                Row = 0,
                Column = 0
            };
        }

        sizeBox.SelectedIndex = (int) tile.TileSize;
        fontBox.Text = tile.FontSize.ToString( );
        ShadowBox.IsChecked = tile.Shadow;
        ImageShadowBox.IsChecked = tile.ImageShadow;

        DockPanel.SetDock(tile, Dock.Right);
        tile.Margin = new Thickness(5, 10, 10, 5);
        mainPanel.Children.Insert(1, tile);
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
        => tile.Shadow = (bool) ShadowBox.IsChecked;

    private void ImageShadowBoxChecked(object o, RoutedEventArgs e)
        => tile.ImageShadow = (bool) ImageShadowBox.IsChecked;

    private void TaskCancel(object o, RoutedEventArgs e)
        => Close( );

    private void TaskOk(object o, RoutedEventArgs e)
    {
        tile.IsEnabled = true;
        Close( );
    }

    private void WindowClosing(object o, CancelEventArgs e)
    {
        tile.Margin = new Thickness(Defaults.Margin);
        mainPanel.Children.Remove(tile);
    }
}
