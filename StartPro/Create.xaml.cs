using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace StartPro;

public partial class Create : Window
{
    public Create( )
    {
        InitializeComponent( );
    }

    private void TileSizeChanged(object o, SelectionChangedEventArgs e)
        => tile.TileSize = (TileType) sizeBox.SelectedIndex;

    private void SelectExe(object o, RoutedEventArgs e)
    {
        OpenFileDialog dialog = new( )
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
        OpenFileDialog dialog = new( )
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
            iconBox.Text = tile.AppIcon;
            OkButton.IsEnabled = true;
        }
        catch { OkButton.IsEnabled = false; }
    }

    private void IconChanged(object o, TextChangedEventArgs e)
    {
        try
        {
            tile.AppIcon = new Uri(iconBox.Text).LocalPath;
        }
        catch { }
    }

    private void NameChanged(object o, TextChangedEventArgs e)
        => tile.AppName = nameBox.Text;

    private void FontChanged(object o, TextChangedEventArgs e)
    {
        try { tile.FontSize = int.Parse(fontBox.Text); }
        catch { tile.FontSize = Defaults.FontSize; }
    }

    private void TaskCancel(object o, RoutedEventArgs e)
        => Close( );

    private void TaskOk(object o, RoutedEventArgs e)
    {
        mainPanel.Children.Remove(tile);
        tile.IsEnabled = true;
        tile.Margin = new Thickness(0);
        TileGrid grid = tile.GetSize( );
        while (!Tile.IsPosEmpty(grid, tile.TileSize))
            grid.Col += 1;
        Grid.SetRowSpan(tile, grid.Row);
        Grid.SetColumnSpan(tile, grid.Col);
        Close( );
    }
}
