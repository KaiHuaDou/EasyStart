using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

using StartPro.Api;

namespace StartPro.Tile;

public partial class ImportApp : Window
{
    public ImportApp( )
    {
        InitializeComponent( );
        TileList.ItemsSource = ConvertTileSet(Tiles);
    }

    public HashSet<AppTile> Tiles { get; } = [];

    private void AddNewTile(string displayName, string path, string arguments)
    {
        Tiles.Add(new AppTile
        {
            AppName = Path.GetFileNameWithoutExtension(displayName),
            AppPath = path,
            Arguments = arguments,
            AppIcon = path,
            TileSize = TileSize.Medium,
            Shadow = App.Settings.UIFlat,
            ImageShadow = App.Settings.UIFlat,
            Row = 0,
            Column = 0,
            IsEnabled = false
        });
    }

    private void AddNewTiles(string[] files)
    {
        foreach (var file in files)
        {
            if (file.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase)
                && Integration.ResolveShortcut(file, out var target, out var arguments))
            {
                AddNewTile(file, target, arguments);
            }
            else
            {
                AddNewTile(file, file, "");
            }
        }
    }

    private void CancelClick(object o, RoutedEventArgs e)
    {
        Tiles.Clear( );
        Close( );
    }

    private static HashSet<string> ConvertTileSet(HashSet<AppTile> x)
    {
        HashSet<string> result = [];
        foreach (var tile in x)
        {
            result.Add(tile.ToString( ));
        }

        return result;
    }

    private void DropTile(object o, DragEventArgs e)
    {
        var data = e.Data;
        if (!data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
        {
            return;
        }

        AddNewTiles(files);
        RefreshTileList( );
    }

    private void ImportClick(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectFiles(out var fileName, "exe"))
        {
            AddNewTiles(fileName);
        }

        RefreshTileList( );
    }

    private void OkClick(object o, RoutedEventArgs e)
    {
        Close( );
    }

    private void RefreshTileList( )
    {
        TileList.ItemsSource = null;
        TileList.ItemsSource = ConvertTileSet(Tiles);
    }
}
