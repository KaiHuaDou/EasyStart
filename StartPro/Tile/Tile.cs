using System;
using System.Windows;
using System.Windows.Controls;

namespace StartPro;

public partial class Tile
{
    public Tile( )
    {
        InitializeComponent( );
        Refresh( );
    }

    public override void Refresh( )
    {
        border.DataContext = this;
        border.CornerRadius = maskBorder.CornerRadius = (CornerRadius) new RadiusConverter( ).Convert(TileSize, null, null, null);
        MinHeight = Height = Convert.ToDouble(new SizeConverter( ).Convert(TileSize, null, "Height", null));
        MinWidth = Width = Convert.ToDouble(new SizeConverter( ).Convert(TileSize, null, "Width", null));
        Margin = new Thickness(Defaults.Margin);
        if (Parent is Canvas && Application.Current.MainWindow is MainWindow window)
        {
            // 重新测量并布局确保 ActualWidth 和 ActualHeight 及时更新，以便移动磁贴至适宜位置
            Measure(new Size(window.Width, window.Height));
            Arrange(new Rect(0, 0, window.DesiredSize.Width, window.DesiredSize.Height));
            if (Owner is not null)
                MoveToSpace(Owner, true);
        }
    }

    public override string ToString( ) => $"{AppName} - {TileSize}";

    private static readonly PropertyMetadata appNameMeta = new("Application");
    private static readonly PropertyMetadata appIconMeta = new(AppIconChanged);
    private static readonly PropertyMetadata appPathMeta = new(Defaults.AppName, AppPathChanged);
    private static readonly PropertyMetadata ImageShadowMeta = new(true);
    private static readonly PropertyMetadata ShadowMeta = new(true);
    public static readonly DependencyProperty AppNameProperty = DependencyProperty.Register("AppName", typeof(string), typeof(Tile), appNameMeta);
    public static readonly DependencyProperty AppIconProperty = DependencyProperty.Register("AppIcon", typeof(string), typeof(Tile), appIconMeta);
    public static readonly DependencyProperty AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), typeof(Tile), appPathMeta);
    public static readonly DependencyProperty ImageShadowProperty = DependencyProperty.Register("TileImageShadow", typeof(bool), typeof(Tile), ImageShadowMeta);
    public static readonly DependencyProperty ShadowProperty = DependencyProperty.Register("TileShadow", typeof(bool), typeof(Tile), ShadowMeta);

    public string AppName
    {
        get => (string) GetValue(AppNameProperty);
        set => SetValue(AppNameProperty, value);
    }

    public string AppIcon
    {
        get => (string) GetValue(AppIconProperty);
        set => SetValue(AppIconProperty, value);
    }

    public string AppPath
    {
        get => (string) GetValue(AppPathProperty);
        set => SetValue(AppPathProperty, value);
    }

    public bool ImageShadow
    {
        get => (bool) GetValue(ImageShadowProperty);
        set
        {
            SetValue(ImageShadowProperty, value);
            TileImageShadow.Opacity = (!App.Program.Settings.Content.UIFlat && value) ? 0.4 : 0;
        }
    }

    public bool Shadow
    {
        get => (bool) GetValue(ShadowProperty);
        set
        {
            SetValue(ShadowProperty, value);
            TileShadow.Opacity = (!App.Program.Settings.Content.UIFlat && value) ? 0.4 : 0;
        }
    }
}
