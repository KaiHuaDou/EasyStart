using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace StartPro.Tile;

public partial class TileBase : UserControl
{
    public TileBase( )
    {
        InitializeComponent( );
        Refresh( );
    }

    private bool _contentLoaded;

    internal Border maskBorder;
    internal Border border;
    internal DropShadowEffect TileShadow;
    internal ContextMenu contextMenu;
    internal MenuItem SizeSmallMenu;
    internal MenuItem SizeMediumMenu;
    internal MenuItem SizeWideMenu;
    internal MenuItem SizeHighMenu;
    internal MenuItem SizeLargeMenu;
    internal MenuItem TileDeleteMenu;

    public void InitializeComponent( )
    {
        if (_contentLoaded)
            return;
        _contentLoaded = true;
        Uri resourceLocater = new("/StartPro;component/Tile/TileBase_UI.xaml", UriKind.Relative);
        Grid root = Application.LoadComponent(resourceLocater) as Grid;
        Content = root;

        maskBorder = root.FindName("maskBorder") as Border;
        border = root.FindName("border") as Border;
        TileShadow = root.FindName("TileShadowEffect") as DropShadowEffect;
        contextMenu = root.FindName("contextMenu") as ContextMenu;
        SizeSmallMenu = root.FindName("SizeSmallMenu") as MenuItem;
        SizeMediumMenu = root.FindName("SizeMediumMenu") as MenuItem;
        SizeWideMenu = root.FindName("SizeWideMenu") as MenuItem;
        SizeHighMenu = root.FindName("SizeHighMenu") as MenuItem;
        SizeLargeMenu = root.FindName("SizeLargeMenu") as MenuItem;
        TileDeleteMenu = root.FindName("TileDeleteMenu") as MenuItem;

        SizeSmallMenu.Click += ToSmallClick;
        SizeMediumMenu.Click += ToMediumClick;
        SizeWideMenu.Click += ToWideClick;
        SizeHighMenu.Click += ToHighClick;
        SizeLargeMenu.Click += ToLargeClick;
        TileDeleteMenu.Click += RemoveTile;
        MouseLeftButtonDown += TileDragStart;
        MouseLeftButtonUp += TileLeftButtonUp;
        MouseMove += TileDragging;
    }

    protected static void TileColorChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) { }

    protected static void TileSizeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        => (o as TileBase).Refresh( );

    protected void TileLeftButtonUp(object o, MouseButtonEventArgs e)
        => TileDragStop(o, e);

    private void RemoveTile(object o, RoutedEventArgs e)
        => (Parent as Panel).Children.Remove(this);
    private void ToSmallClick(object sender, RoutedEventArgs e) => TileSize = TileType.Small;
    private void ToMediumClick(object sender, RoutedEventArgs e) => TileSize = TileType.Medium;
    private void ToWideClick(object sender, RoutedEventArgs e) => TileSize = TileType.Wide;
    private void ToHighClick(object sender, RoutedEventArgs e) => TileSize = TileType.High;
    private void ToLargeClick(object sender, RoutedEventArgs e) => TileSize = TileType.Large;
}
