using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace StartPro.Api;
public class StartMenuApp
{
    public static string UserAppsPath = Environment.ExpandEnvironmentVariables("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static string SystemAppsPath = Environment.ExpandEnvironmentVariables("%ProgramData%\\Microsoft\\Windows\\Start Menu\\Programs");

    public static IEnumerable<StartMenuApp> Search( )
    {
        foreach (string app in Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories))
            yield return Append(app);
        foreach (string app in Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.AllDirectories))
            yield return Append(app);
    }

    private static StartMenuApp Append(string appPath)
    {
        string appName = Path.GetFileNameWithoutExtension(appPath);
        string realPath = Utils.ReadShortcut(appPath);
        return new StartMenuApp
        {
            AppName = Utils.ShortenStr(appName),
            AppPath = realPath,
            AppIcon = PEIcon.Get(realPath)
        };
    }

    public string AppName { get; set; }
    public string AppPath { get; set; }
    public BitmapSource AppIcon { get; set; }
}
