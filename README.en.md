# EasyStart

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/kaihuadou/easystart/build.yml) ![](https://img.shields.io/github/license/kaihuadou/easystart) ![](https://img.shields.io/github/commit-activity/w/kaihuadou/easystart)

| [中文](./README.md) | English |
| :-----------------: | :-----: |

Windows 10 Start screen tiles rewritten using C# + WPF.

- Supports app tiles, text tiles, and image tiles
- Supports seven sizes: small (1x1), medium (2x2), wide (2x4), thin (1x4), tall (4x2), long (4x1), and large (4x4)
- Support custom font size, font, foreground color, background color
- Infinite canvas and Free dragging
- Import from the system start menu

## Shortcuts

Use <kbd>Ctrl</kbd>+<kbd>Win</kbd> to show or hide start screen.

## Configuration Files

All configuration files are portable.

- `settings.json` stores application settings.
- `tiles.xml` stores tiles.

## Download

Get the latest version from the [Release](https://github.com/KaiHuaDou/EasyStart/releases/latest) page.

## Build

Get the latest build via [GitHub Actions](https://github.com/KaiHuaDou/EasyStart/actions).

To build locally, run the following command. Requires the `.NET 9 SDK` or higher:

```bash
git clone --depth=1 https://github.com/KaiHuaDou/EasyStart.git
cd EasyStart
dotnet build
```

To build a release version, run:

```bash
dotnet publish -p:PublishProfile=FolderProfile
```

## Screenshots

Aero style (`UIFlat = false):

![Screenshot](./doc/Screenshot01.png)

Metro style (UIFlat = true):

![Screenshot](./doc/Screenshot02.png)