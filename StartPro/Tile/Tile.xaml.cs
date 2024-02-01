using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

    public static void TileSizeChanged(DependencyObject o, object value)
    {
        Tile tile = o as Tile;
        tile.MinHeight = tile.Height = Convert.ToDouble(new SizeConverter( ).Convert(value, null, "Height", null));
        tile.MinWidth = tile.Width = Convert.ToDouble(new SizeConverter( ).Convert(value, null, "Width", null));
        tile.Margin = new Thickness(Defaults.Margin);
        tile.border.CornerRadius = (CornerRadius) new RadiusConverter( ).Convert(value, null, null, null);
    }

    private static void AppIconChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
        Tile tile = o as Tile;
        string path = e.NewValue as string;
        if (tile.AppIcon == tile.AppName)
        {
            tile.image.Source = IconMgr.Get(path);
        }
        else if (File.Exists(path))
        {
            try { tile.image.Source = new BitmapImage(new Uri(path)); }
            catch { tile.image.Source = IconMgr.Get(path); }
            return;
        }
        tile.image.Source = new BitmapImage( );
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
            }
            catch { }
        }
        TileDragStop(o, e);
    }

    private void OpenTileLocation(object o, RoutedEventArgs e)
    {
        try
        {
            Process.Start(Directory.GetParent(AppPath).FullName);
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
        MoveToSpace(Parent as Panel, false);
    }

    private void RunAsAdmin(object o, RoutedEventArgs e)
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent( );
        WindowsPrincipal principal = new(identity);
        bool isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        try
        {
            Process.Start(new ProcessStartInfo( )
            {
                UseShellExecute = true,
                FileName = AppPath,
                Verb = isAdmin ? "" : "runas"
            });
        }
        catch { }
    }
}
