using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;

namespace StartPro.Api;
public class StartMenuApp
{
    public static HashSet<StartMenuApp> AllApps = [];
    public static string UserAppsPath => Environment.ExpandEnvironmentVariables("%AppData%\\Microsoft\\Windows\\Start Menu\\Programs");
    public static string SystemAppsPath => Environment.ExpandEnvironmentVariables("%ProgramData%\\Microsoft\\Windows\\Start Menu\\Programs");

    public static void SearchAll( )
    {
        AllApps.Clear( );
        string[] UserApps = Directory.GetFiles(UserAppsPath, "*.lnk", SearchOption.AllDirectories);
        string[] SystemApps = Directory.GetFiles(SystemAppsPath, "*.lnk", SearchOption.TopDirectoryOnly);
        foreach (string app in UserApps)
            AllApps.Add(Generate(app));
        foreach (string app in SystemApps)
            AllApps.Add(Generate(app));
    }

    public static StartMenuApp Generate(string appPath)
    {
        return new StartMenuApp
        {
            AppName = Path.GetFileNameWithoutExtension(appPath),
            AppPath = Utils.ReadShortcut(appPath)
        };
    }

    public string AppName { get; set; }
    public string AppPath { get; set; }
    public ImageSource AppIcon => IconMgr.Get(AppPath);
}
