# Desktop Pet 🐾

Windows 桌面宠物，极简 WPF 实现。

## 功能

- 纯 WPF 绘制的可爱角色（零图片依赖）
- 点击互动 / 随机动作 / 自动睡眠
- 透明无边框置顶窗口
- 拖拽移动

## 运行

```powershell
# 需要 .NET 8 SDK
cd E:\Projects\desktop-pet\DesktopPet
dotnet run
```

## 发布

```powershell
dotnet publish -c Release -r win-x64 --self-contained true -o publish
```

生成的 exe 在 `publish/` 目录下，可直接分发。

## 文件

| 文件 | 说明 |
|------|------|
| `MainWindow.xaml` | 宠物 UI（~120行 XAML） |
| `MainWindow.xaml.cs` | 状态机 + 动画（~150行 C#） |
| `App.xaml` / `App.xaml.cs` | 应用入口（各 ~3 行） |
| `DesktopPet.csproj` | 项目配置（~15 行） |

**总计：~5 个文件，~350 行代码，零第三方依赖。**
