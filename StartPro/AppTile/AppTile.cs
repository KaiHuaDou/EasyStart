﻿using System.Windows;
using System.Windows.Controls;
using StartPro.Api;

namespace StartPro.Tile;

public partial class AppTile : TileBase
{
    public AppTile( ) : base( )
    {
        Grid root = Content as Grid;
        InitializeComponent( );
        userControl.Content = null;
        border.Child = RootPanel;

        Utils.MoveItems(ContextMenu, contextMenu);
        Content = root;
    }

    public override string ToString( ) => $"{AppName} - {TileSize}";

    private static readonly PropertyMetadata appNameMeta = new("Application");
    private static readonly PropertyMetadata appIconMeta = new(AppIconChanged);
    private static readonly PropertyMetadata appPathMeta = new(Defaults.AppName, AppPathChanged);
    private static readonly PropertyMetadata ImageShadowMeta = new(true);
    private static readonly PropertyMetadata ShadowMeta = new(true);
    public static readonly DependencyProperty AppNameProperty = DependencyProperty.Register("AppName", typeof(string), typeof(AppTile), appNameMeta);
    public static readonly DependencyProperty AppIconProperty = DependencyProperty.Register("AppIcon", typeof(string), typeof(AppTile), appIconMeta);
    public static readonly DependencyProperty AppPathProperty = DependencyProperty.Register("AppPath", typeof(string), typeof(AppTile), appPathMeta);
    public static readonly DependencyProperty ImageShadowProperty = DependencyProperty.Register("TileImageShadow", typeof(bool), typeof(AppTile), ImageShadowMeta);
    public static readonly DependencyProperty ShadowProperty = DependencyProperty.Register("TileShadow", typeof(bool), typeof(AppTile), ShadowMeta);

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
            if (TileShadow is not null)
                TileShadow.Opacity = (!App.Program.Settings.Content.UIFlat && value) ? 0.4 : 0;
        }
    }
}
