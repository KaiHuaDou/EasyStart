﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using StartPro.Api;

namespace StartPro;
public partial class Import : Window
{
    public HashSet<Tile> Tiles { get; } = [];

    private HashSet<string> ConvertTileSet(HashSet<Tile> x)
    {
        HashSet<string> result = [];
        foreach (Tile tile in x)
            result.Add(tile.ToString( ));
        return result;
    }

    public Import( )
    {
        InitializeComponent( );
        TileList.ItemsSource = ConvertTileSet(Tiles);
    }

    private void OkClick(object o, RoutedEventArgs e) => Close( );

    private void CancelClick(object o, RoutedEventArgs e)
    {
        Tiles.Clear( );
        Close( );
    }

    private void ImportClick(object o, RoutedEventArgs e)
    {
        if (Utils.TrySelectExe(out string fileName, multiSelect: true))
        {
            AddNewTile(fileName);
        }
        RefreshTileList( );
    }

    private void AddNewTile(string fileName)
    {
        Tiles.Add(new Tile
        {
            AppName = Path.GetFileNameWithoutExtension(fileName),
            AppPath = fileName,
            AppIcon = fileName,
            TileSize = TileType.Medium,
            Shadow = App.Program.Settings.Content.UIFlat,
            ImageShadow = App.Program.Settings.Content.UIFlat,
            Row = 0,
            Column = 0,
            IsEnabled = false
        });
    }

    private void DropTile(object o, DragEventArgs e)
    {
        IDataObject data = e.Data;
        if (!data.GetDataPresent(DataFormats.FileDrop))
            return;
        Array files = e.Data.GetData(DataFormats.FileDrop) as Array;
        foreach (object file in files)
        {
            string fileName = file.ToString( );
            if (fileName.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
                fileName = Utils.ReadShortcut(fileName);
            AddNewTile(fileName);
        }
        RefreshTileList( );
    }

    private void RefreshTileList( )
    {
        TileList.ItemsSource = null;
        TileList.ItemsSource = ConvertTileSet(Tiles);
    }
}