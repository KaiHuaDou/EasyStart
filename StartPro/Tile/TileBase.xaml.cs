using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

using DependencyPropertyGenerator;

namespace StartPro.Tile;

[DependencyProperty<TileSize>("TileSize", DefaultValue = TileSize.Medium, OnChanged = nameof(OnTileSizeChanged))]
[DependencyProperty<Brush>("TileColor", DefaultValueExpression = "StartPro.Api.Defaults.TileColorBrush", OnChanged = nameof(OnTileColorChanged))]
[DependencyProperty<bool>("Shadow", DefaultValue = false, OnChanged = nameof(OnShadowChanged))]
public partial class TileBase : UserControl
{
    internal Border border = null!;
    internal ContextMenu contextMenu = null!;
    internal Border maskBorder = null!;
    internal MenuItem SizeHighMenu = null!;
    internal MenuItem SizeLargeMenu = null!;
    internal MenuItem SizeMediumMenu = null!;
    internal MenuItem SizeSmallMenu = null!;
    internal MenuItem SizeTallMenu = null!;
    internal MenuItem SizeThinMenu = null!;
    internal MenuItem SizeWideMenu = null!;
    internal MenuItem TileDeleteMenu = null!;
    internal DropShadowEffect TileShadow = null!;

    private bool _contentLoaded;

    public TileBase( )
    {
        InitializeComponent( );
        OnTileSizeChanged(TileSize);
        OnShadowChanged(Shadow);
        Refresh( );
    }

    public Panel? Owner => Parent as Panel;

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
        {
            return;
        }

        _contentLoaded = true;
        Uri resourceLocater = new("/StartPro;component/Tile/TileBase_UI.xaml", UriKind.Relative);
        var root = Application.LoadComponent(resourceLocater) as Grid ?? throw new ObjectDisposedException("");
        Content = root;

        maskBorder = root.FindName("maskBorder") as Border ?? throw new ObjectDisposedException("");
        border = root.FindName("border") as Border ?? throw new ObjectDisposedException("");
        TileShadow = root.FindName("TileShadow") as DropShadowEffect ?? throw new ObjectDisposedException("");
        contextMenu = root.FindName("contextMenu") as ContextMenu ?? throw new ObjectDisposedException("");
        SizeSmallMenu = root.FindName("SizeSmallMenu") as MenuItem ?? throw new ObjectDisposedException("");
        SizeMediumMenu = root.FindName("SizeMediumMenu") as MenuItem ?? throw new ObjectDisposedException("");
        SizeThinMenu = root.FindName("SizeThinMenu") as MenuItem ?? throw new ObjectDisposedException("");
        SizeWideMenu = root.FindName("SizeWideMenu") as MenuItem ?? throw new ObjectDisposedException("");
        SizeTallMenu = root.FindName("SizeTallMenu") as MenuItem ?? throw new ObjectDisposedException("");
        SizeHighMenu = root.FindName("SizeHighMenu") as MenuItem ?? throw new ObjectDisposedException("");
        SizeLargeMenu = root.FindName("SizeLargeMenu") as MenuItem ?? throw new ObjectDisposedException("");
        TileDeleteMenu = root.FindName("TileDeleteMenu") as MenuItem ?? throw new ObjectDisposedException("");

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
    {
        TileShadow.Opacity = (!App.Settings.UIFlat && newValue) ? 0.4 : 0;
    }

    protected virtual void OnTileColorChanged(Brush newValue) { }

    protected virtual void OnTileSizeChanged(TileSize newValue)
    {
        var tileSize = TileDatas.TileSizes[newValue];
        MinWidth = Width = tileSize.Item1;
        MinHeight = Height = tileSize.Item2;
        Margin = new Thickness(TileDatas.BaseMargin);
        border.CornerRadius = maskBorder.CornerRadius = new CornerRadius(TileDatas.TileRadius[newValue]);
        Refresh( );
    }

    private void RemoveTile(object o, RoutedEventArgs e)
    {
        Owner?.Children.Remove(this);
    }

    private void ToSmallClick(object o, RoutedEventArgs e)
    {
        TileSize = TileSize.Small;
    }

    private void ToMediumClick(object o, RoutedEventArgs e)
    {
        TileSize = TileSize.Medium;
    }

    private void ToThinClick(object o, RoutedEventArgs e)
    {
        TileSize = TileSize.Thin;
    }

    private void ToWideClick(object o, RoutedEventArgs e)
    {
        TileSize = TileSize.Wide;
    }

    private void ToHighClick(object o, RoutedEventArgs e)
    {
        TileSize = TileSize.High;
    }

    private void ToTallClick(object o, RoutedEventArgs e)
    {
        TileSize = TileSize.Tall;
    }

    private void ToLargeClick(object o, RoutedEventArgs e)
    {
        TileSize = TileSize.Large;
    }
}
