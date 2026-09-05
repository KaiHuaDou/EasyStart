using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

using StartPro.Resources;
using StartPro.Tile;

namespace StartPro.Api;

#if WINDOWS7_0_OR_GREATER

public record TileRaw(
    string AppName,
    string AppPath,
    string Arguments,
    string AppIcon,
    TileSize TileSize,
    int Row,
    int Column
);

public static partial class SystemTiles
{
    private static readonly Dictionary<string, TileSize> SizeMap = new( )
    {
        ["1x1"] = TileSize.Small,
        ["2x2"] = TileSize.Medium,
        ["4x2"] = TileSize.Wide,
        ["4x4"] = TileSize.Large
    };

    public static TileBase CreateTile(TileRaw data)
    {
        return new StartPro.Tile.AppTile
        {
            AppName = data.AppName,
            AppPath = data.AppPath,
            Arguments = data.Arguments,
            AppIcon = data.AppIcon,
            TileSize = data.TileSize,
            TileColor = Defaults.TileColorBrush,
            Shadow = false,
            ImageShadow = false,
            FontSize = Defaults.FontSize,
            Row = data.Row,
            Column = data.Column,
        };
    }

    public static Collection<TileRaw> ImportData( )
    {
        if (!GetXml(out var xml))
        {
            return [];
        }

        Collection<TileRaw> result = [];

        var groups = xml
            .Descendants( )
            .Where(static e => e.Name.LocalName == "Group");
        var groupWidthRaw = xml
            .Descendants( )
            .First(static e => e.Name.LocalName == "LayoutOptions")?
            .Attribute("StartTileGroupCellWidth")?
            .Value ?? "8";
        var groupWidth = int.TryParse(groupWidthRaw, out var _width) ? _width : 0;

        int colAdjust = 0, rowAdjust = 0, rowMax = 0;
        foreach (var group in groups)
        {
            var tiles = group
                .Descendants( )
                .Where(static e => e.Name.LocalName is "DesktopApplicationTile" or "Tile");

            foreach (var tile in tiles)
            {
                var colRaw = tile.Attribute("Column")?.Value;
                var rowRaw = tile.Attribute("Row")?.Value;
                var col = int.TryParse(colRaw, out var _col) ? _col : 0;
                var row = int.TryParse(rowRaw, out var _row) ? _row : 0;
                if (row > rowMax)
                {
                    rowMax = row;
                }

                var tileData = ParseTileData(tile, row + rowAdjust, col + colAdjust);
                result.Add(tileData);
            }

            colAdjust += groupWidth + 1;
            if (colAdjust == 3 * (groupWidth + 1))
            {
                rowAdjust += rowMax + 4;
                colAdjust = 0;
            }
        }

        return result;
    }

    [GeneratedRegex(@"(?<=(.+\.)).+(?=_)")]
    private static partial Regex AUMIDRegex( );
    private static string ExtractName(string path)
    {
        path = path.Trim( ).Trim('"');
        var result = "App";
        try
        {
            var fileName = Path.GetFileNameWithoutExtension(path);
            if (!string.IsNullOrEmpty(fileName))
            {
                result = fileName;
            }
        }
        catch { }

        return result;
    }

    private static bool GetXml(out XDocument xml)
    {
        xml = null!;
        var tempFile = Path.Combine(Path.GetTempPath( ), $"StartPro_StartMenuExport_{Random.Shared.Next( )}.xml");
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
            using var process = Process.Start(psi);
            if (process is null)
            {
                App.AddInfo(Info.ExportStartMenuErrorPowerShell);
                return false;
            }

            if (!process.WaitForExit(5000))
            {
                try { process.Kill(true); } catch { }

                App.AddInfo(Info.ExportStartMenuErrorTimeout);
                return false;
            }

            if (process.ExitCode != 0)
            {
                App.AddInfo(string.Format(Info.ExportStartMenuErrorStdErr, process.StandardError.ReadToEnd( )));
                return false;
            }

            if (!File.Exists(tempFile))
            {
                App.AddInfo(Info.ExportStartMenuErrorNoFile);
                return false;
            }

            xml = XDocument.Load(tempFile);
            return true;
        }
        catch (Exception ex)
        {
            App.AddInfo(string.Format(Info.ExportStartMenuErrorException, ex.Message));
            return false;
        }
        finally
        {
            try { File.Delete(tempFile); } catch { }
        }
    }

    private static TileRaw ParseTileData(XElement tile, int row, int col)
    {
        var size = SizeMap[tile.Attribute("Size")?.Value ?? "2x2"];
        string name, path, arguments, icon;
        path = arguments = icon = string.Empty;
        if (tile.Name.LocalName == "Tile")
        {
            var id = tile.Attribute("AppUserModelID")!.Value;
            path = "explorer.exe";
            arguments = $"shell:AppsFolder\\{id}";
            icon = arguments;
            name = AUMIDRegex( ).Match(id).Groups[0].Value;
        }
        else
        {
            var lnk = tile.Attribute("DesktopApplicationLinkPath")?.Value.Trim('"')!;
            lnk = Path.GetFullPath(Environment.ExpandEnvironmentVariables(lnk));
            name = ExtractName(lnk);
            if (Integration.ResolveShortcut(lnk, out var _path, out var _args))
            {
                path = _path.Contains(@"\Windows\Installer\{", StringComparison.OrdinalIgnoreCase) ? lnk : _path;
                arguments = _args;
                icon = string.IsNullOrEmpty(arguments) ? path : lnk;
            }
        }

        return new TileRaw(name, path, arguments, icon, size, row, col);
    }
}
#endif

#if WINDOWS
public class SystemApp
{
    public static ReadOnlyCollection<SystemApp> Apps = new(Array.Empty<SystemApp>( ));
    private readonly static string SystemAppsPath = Environment.ExpandEnvironmentVariables("%ProgramData%\\Microsoft\\Windows\\Start Menu\\Programs");
    private readonly static string UserAppsPath = Environment.ExpandEnvironmentVariables("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs");

    private IntPtr appIcon;
    public BitmapSource AppIcon { get; set; } = new BitmapImage( );

    public string AppName { get; set; } = string.Empty;

    public string AppPath { get; set; } = string.Empty;

    public string Arguments { get; set; } = string.Empty;

    public static void LoadApps( )
    {
        var UserApps = Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories);
        var SystemApps = Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.AllDirectories);
        Apps = new([.. UserApps.Concat(SystemApps).Select(FromLazy)]);
    }

    public static void LoadIcon( )
    {
        foreach (var app in Apps)
        {
            app.AppIcon = PEIcon.FromIcon(Icon.FromHandle(app.appIcon), out var source)
                ? source ?? new BitmapImage( )
                : PEIcon.FromBitmap(app.appIcon, out var source1)
                    ? source1 ?? new BitmapImage( )
                    : new BitmapImage( );
            app.AppIcon.Freeze( );
        }
    }

    private static SystemApp FromLazy(string appPath)
    {
        var appName = Path.GetFileNameWithoutExtension(appPath);
        return appPath.EndsWith("*.lnk", StringComparison.InvariantCultureIgnoreCase)
            && Integration.ResolveShortcut(appPath, out var target, out var arguments)
            ? new SystemApp
            {
                AppName = appName,
                AppPath = target ?? string.Empty,
                Arguments = arguments ?? string.Empty,
                appIcon = PEIcon.DirectIcon(target ?? string.Empty)?.Handle ?? IntPtr.Zero,
            }
            : new SystemApp
            {
                AppName = appName,
                AppPath = appPath,
                Arguments = string.Empty,
                appIcon = PEIcon.ComplexBitmap(appPath)
            };
    }
}
#endif
