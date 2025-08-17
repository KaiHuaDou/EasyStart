using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using StartPro.Tile;

namespace StartPro.Api;

public static class SystemTiles
{
    private static readonly Dictionary<string, TileSize> SizeMap = new( )
    {
        ["1x1"] = TileSize.Small,
        ["2x2"] = TileSize.Medium,
        ["2x4"] = TileSize.Wide,
        ["4x4"] = TileSize.Large
    };

    public static ObservableCollection<TileBase> Import( )
    {
        XDocument xml = GetXml( );
        ObservableCollection<TileBase> result = [];

        IEnumerable<XElement> groups = xml
            .Descendants( )
            .Where(static e => e.Name.LocalName == "Group");
        string groupWidthRaw = xml.Descendants( ).First(static e => e.Name.LocalName == "LayoutOptions")?.Attribute("StartTileGroupCellWidth")?.Value;
        int groupWidth = int.TryParse(groupWidthRaw, out int _width) ? _width : 0;

        int colAdjust = 0, rowAdjust = 0, rowMax = 0;
        foreach (XElement group in groups)
        {
            IEnumerable<XElement> tiles = group
                .Descendants( )
                .Where(static e => e.Name.LocalName is "DesktopApplicationTile" or "Tile");

            foreach (XElement tile in tiles)
            {
                TileSize size = SizeMap[tile.Attribute("Size").Value];

                string colRaw = tile.Attribute("Column")?.Value;
                string rowRaw = tile.Attribute("Row")?.Value;
                int col = int.TryParse(colRaw, out int _col) ? _col : 0;
                int row = int.TryParse(rowRaw, out int _row) ? _row : 0;
                if (row > rowMax) rowMax = row;

                if (tile.Name.LocalName == "Tile")
                {
                    string text = tile.Attribute("AppUserModelID")?.Value ?? string.Empty;
                    text = text.Replace("!App", string.Empty).Replace('.', '\n').Replace('_', '\n');
                    TextTile textTile = new( )
                    {
                        TileSize = size,
                        Text = text,
                        TileColor = Defaults.TileColor,
                        Shadow = false,
                        FontSize = Defaults.FontSize,
                        Row = row + rowAdjust,
                        Column = col + colAdjust,
                    };
                    result.Add(textTile);
                    continue;
                }

                string lnk = tile.Attribute("DesktopApplicationLinkPath")?.Value.Trim('"') ?? string.Empty;
                lnk = Path.GetFullPath(Environment.ExpandEnvironmentVariables(lnk));
                string name = ExtractName(lnk);
                string path = Utils.ResolveShortcut(lnk);

                AppTile appTile = new( )
                {
                    AppName = name,
                    AppPath = path,
                    TileSize = size,
                    TileColor = Defaults.TileColor,
                    Shadow = false,
                    ImageShadow = false,
                    FontSize = Defaults.FontSize,
                    Row = row + rowAdjust,
                    Column = col + colAdjust * groupWidth,
                };
                result.Add(appTile);
            }
            colAdjust++;
            if (colAdjust == 3)
            {
                rowAdjust += rowMax + 4;
                colAdjust = 0;
            }
        }
        return result;
    }

    private static string ExtractName(string path)
    {
        path = path.Trim( ).Trim('"');
        string result = "App";
        try
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (!string.IsNullOrEmpty(fileName))
                result = fileName;
        }
        catch { }
        return result;
    }

    private static XDocument GetXml( )
    {
        string tempFile = Path.Combine(Path.GetTempPath( ), $"StartPro_StartMenuExport_{Random.Shared.Next( )}.xml");
        ProcessStartInfo psi = new( )
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -NonInteractive -ExecutionPolicy Bypass -Command \"Export-StartLayout -Path '{tempFile}'\"",
            RedirectStandardOutput = false,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        try
        {
            using Process process = Process.Start(psi);
            process.WaitForExit( );

            if (process.ExitCode != 0)
            {
                string err = process.StandardError.ReadToEnd( );
                App.AddInfo($"导出开始菜单错误 {process.ExitCode}: {err}");
            }
            return XDocument.Load(tempFile);
        }
        finally
        {
            try
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
            catch { }
        }
    }
}
public class SystemApp
{
    public static string UserAppsPath = Environment.ExpandEnvironmentVariables("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static string SystemAppsPath = Environment.ExpandEnvironmentVariables("%ProgramData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static ReadOnlyCollection<SystemApp> Apps;

    public static void LoadApps( )
    {
        Apps = new ReadOnlyCollection<SystemApp>(
            [.. Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.AllDirectories))
                    .Select(FromLazy)]
        );
    }

    public static void LoadIcon( )
    {
        foreach (SystemApp app in Apps)
        {
            app.AppIcon =
                PEIcon.FromIcon(app.appIcon, out BitmapSource source)
                ? source : new BitmapImage( );
        }
    }

    private static SystemApp FromLazy(string appPath)
    {
        string appName = Path.GetFileNameWithoutExtension(appPath);
        string realPath = Utils.ResolveShortcut(appPath);
        return new SystemApp
        {
            AppName = Utils.ShortenStr(appName),
            AppPath = realPath,
            appIcon = PEIcon.GetIcon(realPath),
        };
    }

    public string AppName { get; set; }
    public string AppPath { get; set; }

    private Icon appIcon;
    public BitmapSource AppIcon { get; set; }
}
