using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace StartPro.Tile;

public partial class TileBase : UserControl
{
    internal Border border;
    internal ContextMenu contextMenu;
    internal Border maskBorder;
    internal MenuItem SizeHighMenu;
    internal MenuItem SizeLargeMenu;
    internal MenuItem SizeThinMenu;
    internal MenuItem SizeMediumMenu;
    internal MenuItem SizeSmallMenu;
    internal MenuItem SizeTallMenu;
    internal MenuItem SizeWideMenu;
    internal MenuItem TileDeleteMenu;
    internal DropShadowEffect TileShadow;

    private bool _contentLoaded;

    public TileBase( )
    {
        InitializeComponent( );
    }

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
        TileShadow = root.FindName("TileShadow") as DropShadowEffect;
        contextMenu = root.FindName("contextMenu") as ContextMenu;
        SizeSmallMenu = root.FindName("SizeSmallMenu") as MenuItem;
        SizeMediumMenu = root.FindName("SizeMediumMenu") as MenuItem;
        SizeThinMenu = root.FindName("SizeThinMenu") as MenuItem;
        SizeWideMenu = root.FindName("SizeWideMenu") as MenuItem;
        SizeTallMenu = root.FindName("SizeTallMenu") as MenuItem;
        SizeHighMenu = root.FindName("SizeHighMenu") as MenuItem;
        SizeLargeMenu = root.FindName("SizeLargeMenu") as MenuItem;
        TileDeleteMenu = root.FindName("TileDeleteMenu") as MenuItem;

        SizeSmallMenu.Click += ToSmallClick;
        SizeMediumMenu.Click += ToMediumClick;
        SizeThinMenu.Click += ToThinClick;
        SizeWideMenu.Click += ToWideClick;
        SizeTallMenu.Click += ToTallClick;
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

    private void ToSmallClick(object o, RoutedEventArgs e) => TileSize = TileSize.Small;
    private void ToMediumClick(object o, RoutedEventArgs e) => TileSize = TileSize.Medium;
    private void ToThinClick(object o, RoutedEventArgs e) => TileSize = TileSize.Thin;
    private void ToWideClick(object o, RoutedEventArgs e) => TileSize = TileSize.Wide;
    private void ToTallClick(object o, RoutedEventArgs e) => TileSize = TileSize.Tall;
    private void ToHighClick(object o, RoutedEventArgs e) => TileSize = TileSize.High;
    private void ToLargeClick(object o, RoutedEventArgs e) => TileSize = TileSize.Large;
}
