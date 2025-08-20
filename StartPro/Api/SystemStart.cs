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
using StartPro.Tile;

namespace StartPro.Api;

#if WINDOWS

public static partial class SystemTiles
{
    private static readonly Dictionary<string, TileSize> SizeMap = new( )
    {
        ["1x1"] = TileSize.Small,
        ["2x2"] = TileSize.Medium,
        ["2x4"] = TileSize.Wide,
        ["4x4"] = TileSize.Large
    };

    private static bool GetXml(out XDocument xml)
    {
        xml = null!;
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
            if (process is null)
            {
                App.AddInfo("导出开始菜单错误 - 无法启动 Powershell");
                return false;
            }
            if (!process.WaitForExit(5000))
            {
                try { process.Kill(true); } catch { }
                App.AddInfo("导出开始菜单错误 - 进程超时");
                return false;
            }
            if (process.ExitCode != 0)
            {
                App.AddInfo($"导出开始菜单错误: {process.StandardError.ReadToEnd( )}");
                return false;
            }
            if (!File.Exists(tempFile))
            {
                App.AddInfo("导出开始菜单错误 - 未生成文件");
                return false;
            }
            xml = XDocument.Load(tempFile);
            return true;
        }
        catch (Exception ex)
        {
            App.AddInfo($"导出开始菜单错误: {ex.Message}");
            return false;
        }
        finally
        {
            try { File.Delete(tempFile); } catch { }
        }
    }

    public static List<TileBase> Import( )
    {
        if (!GetXml(out XDocument xml))
            return [];
        List<TileBase> result = [];

        IEnumerable<XElement> groups = xml
            .Descendants( )
            .Where(static e => e.Name.LocalName == "Group");
        string groupWidthRaw = xml
            .Descendants( )
            .First(static e => e.Name.LocalName == "LayoutOptions")?
            .Attribute("StartTileGroupCellWidth")?
            .Value ?? "8";
        int groupWidth = int.TryParse(groupWidthRaw, out int _width) ? _width : 0;

        int colAdjust = 0, rowAdjust = 0, rowMax = 0;
        foreach (XElement group in groups)
        {
            IEnumerable<XElement> tiles = group
                .Descendants( )
                .Where(static e => e.Name.LocalName is "DesktopApplicationTile" or "Tile");

            foreach (XElement tile in tiles)
            {
                string colRaw = tile.Attribute("Column")!.Value;
                string rowRaw = tile.Attribute("Row")!.Value;
                int col = int.TryParse(colRaw, out int _col) ? _col : 0;
                int row = int.TryParse(rowRaw, out int _row) ? _row : 0;
                if (row > rowMax) rowMax = row;

                AppTile appTile = ParseTile(tile, row + rowAdjust, col + colAdjust);
                result.Add(appTile);
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

    private static AppTile ParseTile(XElement tile, int row, int col)
    {
        TileSize size = SizeMap[tile.Attribute("Size")!.Value];
        string name, path, arguments, icon;
        path = arguments = icon = string.Empty;
        if (tile.Name.LocalName == "Tile")
        {
            string id = tile.Attribute("AppUserModelID")?.Value!;
            path = "explorer.exe";
            arguments = $"shell:AppsFolder\\{id}";
            icon = arguments;
            name = AUMIDRegex( ).Match(id).Groups[0].Value;
        }
        else
        {
            string lnk = tile.Attribute("DesktopApplicationLinkPath")?.Value.Trim('"')!;
            lnk = Path.GetFullPath(Environment.ExpandEnvironmentVariables(lnk));
            name = ExtractName(lnk);
            if (Integration.ResolveShortcut(lnk, out string _path, out string _args))
            {
                path = _path.Contains(@"\Windows\Installer\{") ? lnk : _path;
                arguments = _args;
                icon = string.IsNullOrEmpty(arguments) ? path : lnk;
            }
        }
        return new( )
        {
            AppName = name,
            AppPath = path,
            Arguments = arguments,
            AppIcon = icon,
            TileSize = size,
            TileColor = Defaults.TileColor,
            Shadow = false,
            ImageShadow = false,
            FontSize = Defaults.FontSize,
            Row = row,
            Column = col,
        };
    }
}

public class SystemApp
{
    public static ReadOnlyCollection<SystemApp> Apps;
    public static string SystemAppsPath = Environment.ExpandEnvironmentVariables("%ProgramData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static string UserAppsPath = Environment.ExpandEnvironmentVariables("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs");

    private Icon appIcon;
    public BitmapSource AppIcon { get; set; }

    public string AppName { get; set; }

    public string AppPath { get; set; }

    public string Arguments { get; set; } = string.Empty;

    public static void LoadApps( )
    {
        Apps = new ReadOnlyCollection<SystemApp>(
            [.. Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.AllDirectories))
                    .Select(FromLazy).Where(static item => item is not null)]
        );
    }

    public static void LoadIcon( )
    {
        foreach (SystemApp app in Apps)
        {
            app.AppIcon =
                PEIcon.FromIcon(app.appIcon, out BitmapSource source)
                ? source : new BitmapImage( );
            app.AppIcon.Freeze( );
        }
    }

    private static SystemApp FromLazy(string appPath)
    {
        string appName = Path.GetFileNameWithoutExtension(appPath);
        return !Integration.ResolveShortcut(appPath, out string target, out string arguments)
            ? null
            : new SystemApp
            {
                AppName = Utils.ShortenStr(appName),
                AppPath = target,
                Arguments = arguments,
                appIcon = PEIcon.GetDirect(target),
            };
    }
}

#endif
