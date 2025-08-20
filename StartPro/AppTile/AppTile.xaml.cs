using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using DependencyPropertyGenerator;
using StartPro.Api;

namespace StartPro.Tile;

[DependencyProperty<string>("AppName", DefaultValue = "Application")]
[DependencyProperty<string>("AppIcon")]
[DependencyProperty<string>("AppPath", DefaultValue = @"%WINDIR%\System32\notepad.exe")]
[DependencyProperty<string>("Arguments", DefaultValue = "")]
[DependencyProperty<bool>("ImageShadow", DefaultValue = false)]
public partial class AppTile : TileBase, IEditable<AppTile>
{
    static AppTile( )
    {
        FrameworkPropertyMetadata tileSizeMeta = new(
            TileSize.Medium,
            static (sender, args)
                => (sender as AppTile)?.OnTileSizeChanged((TileSize) args.NewValue)
        );
        TileSizeProperty.OverrideMetadata(typeof(AppTile), tileSizeMeta);
    }

    public AppTile( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        Utils.AppendContexts(ContextMenu, contextMenu);

        MouseLeftButtonUp += TileLeftButtonUp;

        userControl.Content = null;
        border.Child = RootPanel;
        Content = root;

        Foreground = Utils.TryParseBrushFromText(App.Settings.Foreground, out Brush fore)
            ? fore : Defaults.Foreground;
    }

    public IEditor<AppTile> Editor => new CreateApp(this);

    public override void ReadAttributes(XmlNode node)
    {
        base.ReadAttributes(node);
        FontSize = node.FromAttribute("FontSize", Defaults.FontSize);
        AppPath = node.FromAttribute("Path", Environment.ProcessPath!);
        Arguments = node.FromAttribute("Arguments", string.Empty);
        AppName = node.FromAttribute("Name", "StartPro");
        AppIcon = node.FromAttribute("Icon", Environment.ProcessPath!);
        ImageShadow = node.FromAttribute("ImageShadow", false);
    }

    public override void WriteAttributes(ref XmlElement element)
    {
        base.WriteAttributes(ref element);
        element.SetAttribute("Type", "AppTile");
        element.SetAttribute("Name", AppName);
        element.SetAttribute("Path", AppPath);
        element.SetAttribute("Arguments", Arguments);
        element.SetAttribute("Icon", AppIcon);
        element.SetAttribute("ImageShadow", ImageShadow.ToString( ));
        element.SetAttribute("FontSize", FontSize.ToString( ));
    }

    protected new void OnTileSizeChanged(TileSize newValue)
    {
        base.OnTileSizeChanged(newValue);
        TileLabel.Visibility =
            newValue is TileSize.Small or TileSize.Thin or TileSize.Tall
            ? Visibility.Collapsed : Visibility.Visible;
    }

    private void EditTile(object o, RoutedEventArgs e)
    {
        (this as IEditable<AppTile>).Edit(Owner);
    }

    partial void OnAppIconChanged(string newValue)
    {
        image.Source = Utils.ParseImageSource(newValue);
    }

    partial void OnAppPathChanged(string newValue)
    {
        AppIcon = newValue;
    }

    partial void OnImageShadowChanged(bool newValue)
    {
        TileImageShadow.Opacity = (!App.Settings.UIFlat && newValue) ? 0.4 : 0;
    }

    private void OpenTileLocation(object o, RoutedEventArgs e)
    {
        try
        {
            Integration.ExecuteAsAdmin("explorer.exe", $"/e, /select, {AppPath}");
            App.TileWindow.Hide( );
        }
        catch (Win32Exception)
        {
            App.AddInfo("无法打开应用所在目录");
        }
    }

    private void RunAsAdmin(object o, RoutedEventArgs e)
    {
        Integration.ExecuteAsAdmin(AppPath);
        App.TileWindow.Hide( );
    }

    private void TileLeftButtonUp(object o, MouseButtonEventArgs e)
    {
        if (IsDragging || !IsEnabled)
            return;

        try
        {
            Process.Start(new ProcessStartInfo( )
            {
                UseShellExecute = true,
                FileName = AppPath,
                Arguments = Arguments
            });
        }
        catch (Exception ex)
        {
            App.AddInfo($"无法启动程序: {ex.Message}");
        }
        App.TileWindow.Hide( );
    }
}
