# EasyStart

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/kaihuadou/easystart/build.yml) ![](https://img.shields.io/github/license/kaihuadou/easystart) ![](https://img.shields.io/github/commit-activity/w/kaihuadou/easystart)

Windows 磁贴开始菜单的替代品

- 支持应用磁贴、文本磁贴、图像磁贴
- 支持小 (1x1)、中 (2x2)、宽 (2x4)、薄 (1x4)、高 (4x2)、长 (4x1)、大 (4x4) 七种尺寸
- 无限画布、自由拖动
- 可从开始菜单快速固定应用磁贴

## 配置文件

所有配置文件都是便携的

- `settings.xml` 保存应用程序设置
- `tiles.xml` 保存磁贴
    - 手动修改可能导致磁贴丢失

## 下载

[Release](https://github.com/KaiHuaDou/EasyStart/releases/latest) 页面获取最新版本

## 构建

通过 [GitHub Actions](https://github.com/KaiHuaDou/EasyStart/actions) 获取最新构建

运行以下代码即可本地构建，需要`.NET 9.0 SDK`或更高版本：

```bash
git clone --depth=1 https://github.com/KaiHuaDou/EasyStart.git
cd EasyStart
dotnet build
# 单文件输出
# dotnet publish -p:PublishProfile=FolderProfile
```

## 效果

![效果](./doc/Effect.png)
