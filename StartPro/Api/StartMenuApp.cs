using System;
using System.Collections.ObjectModel;
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
        Apps = new ReadOnlyCollection<StartMenuApp>(
            [.. Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.AllDirectories))
                    .Select(FromLazy)]
        );
    }

    public static void LoadIcon( )
    {
        foreach (StartMenuApp app in Apps)
        {
            app.AppIcon =
                PEIcon.FromIcon(app.appIcon, out BitmapSource source)
                ? source : new BitmapImage( );
        }
    }

    private static StartMenuApp FromLazy(string appPath)
    {
        string appName = Path.GetFileNameWithoutExtension(appPath);
        string realPath = Utils.ReadShortcut(appPath);
        return new StartMenuApp
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
