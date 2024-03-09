﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using StartPro.Api;

namespace StartPro;

public partial class Tile : UserControl
{
    public static void AppPathChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        Tile tile = o as Tile;
        string value = e.NewValue as string;
        FileInfo app = new(value);
        tile.AppName = app.Name.Replace(app.Extension, "");
        tile.AppIcon = value;

    }

    public static void TileColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        Tile tile = o as Tile;
        Color color = tile.TileColor.Color;
        int brightness = (int) (color.R * 0.299) + (int) (color.G * 0.587) + (int) (color.B * 0.114);
        tile.label.Foreground = new SolidColorBrush(brightness > 128 ? Colors.Black : Colors.White);
    }

    public static void TileSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        Tile tile = o as Tile;
        TileType tileType = (TileType) e.NewValue;
        tile.MinHeight = tile.Height = Convert.ToDouble(new SizeConverter( ).Convert(tileType, null, "Height", null));
        tile.MinWidth = tile.Width = Convert.ToDouble(new SizeConverter( ).Convert(tileType, null, "Width", null));
        tile.border.CornerRadius = tile.maskBorder.CornerRadius = (CornerRadius) new RadiusConverter( ).Convert(tileType, null, null, null);
        tile.Margin = new Thickness(Defaults.Margin);
        if (Application.Current.MainWindow is MainWindow window)
        {
            // 重新测量并布局确保 ActualWidth 和 ActualHeight 及时更新，以便移动磁贴至适宜位置
            tile.Measure(new Size(window.Width, window.Height));
            tile.Arrange(new Rect(0, 0, window.DesiredSize.Width, window.DesiredSize.Height));
            if (tile.Owner is not null)
                tile.MoveToSpace(tile.Owner, true);
        }
    }

    private static void AppIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        Tile tile = o as Tile;
        string path = e.NewValue as string;
        try
        {
            if (tile.AppIcon.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                tile.image.Source = IconMgr.Get(path);
            }
            else if (File.Exists(path))
            {
                tile.image.Source = new BitmapImage(new Uri(path));
            }
        }
        catch
        {
            tile.image.Source = new BitmapImage( );
        }
    }

    private void TileLeftButtonUp(object o, MouseButtonEventArgs e)
    {
        if (!IsDragged && IsEnabled)
        {
            try
            {
                Process.Start(new ProcessStartInfo( )
                {
                    UseShellExecute = true,
                    FileName = AppPath,
                });
                (Application.Current.MainWindow as MainWindow).Hide( );
            }
            catch { }
        }
        TileDragStop(o, e);
    }

    private void OpenTileLocation(object o, RoutedEventArgs e)
    {
        try
        {
            Utils.ExecuteAsAdmin(Directory.GetParent(AppPath).FullName);
            (Application.Current.MainWindow as MainWindow).Hide( );
        }
        catch (Win32Exception ex)
        {
            MessageBox.Show(ex.Message, "StartPro",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void RemoveTile(object o, RoutedEventArgs e)
        => ((Panel) Parent).Children.Remove(this);

    private void EditTile(object o, RoutedEventArgs e)
    {
        Panel parent = (Panel) Parent;
        parent.Children.Remove(this);
        Create c = new(this);
        c.ShowDialog( );
        parent.Children.Add(this);
        MoveToSpace(Parent as Panel, true);
    }

    private void RunAsAdmin(object o, RoutedEventArgs e)
    {
        Utils.ExecuteAsAdmin(AppPath);
        (Application.Current.MainWindow as MainWindow).Hide( );

    }

    private void ToSmallClick(object sender, RoutedEventArgs e) => TileSize = TileType.Small;
    private void ToMediumClick(object sender, RoutedEventArgs e) => TileSize = TileType.Medium;
    private void ToWideClick(object sender, RoutedEventArgs e) => TileSize = TileType.Wide;
    private void ToHighClick(object sender, RoutedEventArgs e) => TileSize = TileType.High;
    private void ToLargeClick(object sender, RoutedEventArgs e) => TileSize = TileType.Large;
}
