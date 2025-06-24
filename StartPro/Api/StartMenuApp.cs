using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace StartPro.Api;
public class StartMenuApp
{
    public static string UserAppsPath = Environment.ExpandEnvironmentVariables("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static string SystemAppsPath = Environment.ExpandEnvironmentVariables("%ProgramData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static ReadOnlyCollection<StartMenuApp> Apps;

    public static void LoadApps( )
    {
#if DEBUG
        Apps = new([]);
#else
        Apps = new ReadOnlyCollection<StartMenuApp>(
            [.. Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.AllDirectories))
                    .Select(FromLazy)]
        );
#endif
    }

    public static void LoadIcon( )
    {
        foreach (StartMenuApp app in Apps)
        {
            app.AppIcon = PEIcon.ToSource(app.appIconInner);
        }
    }

#pragma warning disable IDE0051
    private static StartMenuApp FromLazy(string appPath)
    {
        string appName = Path.GetFileNameWithoutExtension(appPath);
        string realPath = Utils.ReadShortcut(appPath);
        return new StartMenuApp
        {
            AppName = Utils.ShortenStr(appName),
            AppPath = realPath,
            appIconInner = PEIcon.GetIcon(realPath),
        };
    }
#pragma warning restore IDE0051

    public string AppName { get; set; }
    public string AppPath { get; set; }

    private Icon appIconInner;
    public BitmapSource AppIcon { get; set; }
}
