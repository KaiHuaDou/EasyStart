using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using DependencyPropertyGenerator;

namespace StartPro.Tile;

[DependencyProperty<TileSize>("TileSize", DefaultValue = TileSize.Medium, OnChanged = nameof(OnTileSizeChanged))]
[DependencyProperty<Brush>("TileColor", DefaultValueExpression = "StartPro.Api.Defaults.TileColor", OnChanged = nameof(OnTileColorChanged))]
[DependencyProperty<bool>("Shadow", DefaultValue = false, OnChanged = nameof(OnShadowChanged))]
public partial class TileBase : UserControl
{
    internal Border border;
    internal ContextMenu contextMenu;
    internal Border maskBorder;
    internal MenuItem SizeHighMenu;
    internal MenuItem SizeLargeMenu;
    internal MenuItem SizeMediumMenu;
    internal MenuItem SizeSmallMenu;
    internal MenuItem SizeTallMenu;
    internal MenuItem SizeThinMenu;
    internal MenuItem SizeWideMenu;
    internal MenuItem TileDeleteMenu;
    internal DropShadowEffect TileShadow;

    private bool _contentLoaded;

    public TileBase( )
    {
        InitializeComponent( );
        OnTileSizeChanged(TileSize);
        OnShadowChanged(Shadow);
        Refresh( );
    }

    public Panel Owner => Parent as Panel;

    public int Column
    {
        get => (int) Canvas.GetLeft(this) / TileDatas.BlockSize;
        set => Canvas.SetLeft(this, (value < 0 ? 0 : value) * TileDatas.BlockSize);
    }

    public int Row
    {
        get => (int) Canvas.GetTop(this) / TileDatas.BlockSize;
        set => Canvas.SetTop(this, (value < 0 ? 0 : value) * TileDatas.BlockSize);
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

        VisualCacheMode = CacheMode = new BitmapCache(1) { SnapsToDevicePixels = true };

        SizeSmallMenu.Click += ToSmallClick;
        SizeMediumMenu.Click += ToMediumClick;
        SizeThinMenu.Click += ToThinClick;
        SizeWideMenu.Click += ToWideClick;
        SizeTallMenu.Click += ToTallClick;
        SizeHighMenu.Click += ToHighClick;
        SizeLargeMenu.Click += ToLargeClick;
        TileDeleteMenu.Click += RemoveTile;
        PreviewMouseLeftButtonDown += TileDragStart;
        PreviewMouseLeftButtonUp += TileDragStop;
        PreviewMouseMove += TileDragging;
    }

    public void Refresh( )
    {
        border.DataContext = this;
        if (Application.Current.MainWindow is MainWindow window)
        {
            // 重新测量并布局确保 ActualWidth 和 ActualHeight 及时更新，以便移动磁贴至适宜位置
            Measure(new Size(window.Width, window.Height));
            Arrange(new Rect(0, 0, window.DesiredSize.Width, window.DesiredSize.Height));
        }
        if (Owner is Canvas owner)
        {
            MoveToSpace( );
            owner.ResizeToFit( );
        }
    }

    protected virtual void OnShadowChanged(bool newValue)
        => TileShadow.Opacity = (!App.Settings.UIFlat && newValue) ? 0.4 : 0;

    protected virtual void OnTileColorChanged(Brush newValue) { }

    protected virtual void OnTileSizeChanged(TileSize newValue)
    {
        (int, int) tileSize = TileDatas.TileSizes[newValue];
        MinWidth = Width = tileSize.Item1;
        MinHeight = Height = tileSize.Item2;
        Margin = new Thickness(TileDatas.BaseMargin);
        border.CornerRadius = maskBorder.CornerRadius = new CornerRadius(TileDatas.TileRadius[newValue]);
        Refresh( );
    }

    private void RemoveTile(object o, RoutedEventArgs e)
        => Owner.Children.Remove(this);

    private void ToSmallClick(object o, RoutedEventArgs e) => TileSize = TileSize.Small;
    private void ToMediumClick(object o, RoutedEventArgs e) => TileSize = TileSize.Medium;
    private void ToThinClick(object o, RoutedEventArgs e) => TileSize = TileSize.Thin;
    private void ToWideClick(object o, RoutedEventArgs e) => TileSize = TileSize.Wide;
    private void ToHighClick(object o, RoutedEventArgs e) => TileSize = TileSize.High;
    private void ToTallClick(object o, RoutedEventArgs e) => TileSize = TileSize.Tall;
    private void ToLargeClick(object o, RoutedEventArgs e) => TileSize = TileSize.Large;
}
