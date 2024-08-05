using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace StartPro.Api;
public class StartMenuApp
{
    public static Dictionary<string, StartMenuApp> AllApps = [];
    public static string UserAppsPath = Environment.ExpandEnvironmentVariables("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static string SystemAppsPath = Environment.ExpandEnvironmentVariables("%ProgramData%\\Microsoft\\Windows\\Start Menu\\Programs");

    public static void SearchAll( )
    {
        AllApps.Clear( );
        string[] UserApps = Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories);
        string[] SystemApps = Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.TopDirectoryOnly);
        foreach (string app in UserApps)
            AppendToList(app);
        foreach (string app in SystemApps)
            AppendToList(app);
    }

    private static void AppendToList(string appPath)
    {
        string appName = Path.GetFileNameWithoutExtension(appPath);
        AllApps.Add(appName, new StartMenuApp
        {
            AppName = appName,
            AppPath = Utils.ReadShortcut(appPath)
        });
    }

    public string AppName { get; set; }
    public string AppPath { get; set; }
    public ImageSource AppIcon => IconMgr.Get(AppPath);
}
