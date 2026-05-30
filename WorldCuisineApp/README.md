# World Cuisine Explorer (.NET MAUI)

世界美食信息应用，适用于作业演示与视频录制。

## 运行

> **注意**：必须在 `WorldCuisineApp` 目录内构建，或打开上级目录的 `WorldCuisine.sln`。  

### 环境

- .NET 9 SDK + MAUI workload
- Visual Studio 2022（含「使用 .NET 的移动开发」工作负载）或 `dotnet workload install maui`

### Windows

```powershell
cd WorldCuisineApp
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

或在 Visual Studio 中选择 **Windows Machine** 后 F5。

### Android

连接设备或模拟器后：

```powershell
dotnet build -f net9.0-android
dotnet build -t:Run -f net9.0-android
```

## MockAPI

1. 在 [mockapi.io](https://mockapi.io) 创建项目，添加资源 **`cuisines`**。
2. 字段示例：`id`, `name`, `country`, `region`, `description`, `imageUrl`, `spiceLevel`, `funFact`（与 `Models/CuisineItem.cs` 一致）。
3. 在应用 **Settings** 中粘贴完整 URL，例如：  
   `https://xxxxxxxx.mockapi.io/cuisines`
4. 未配置或网络失败时，自动使用 `Resources/Raw/cuisines_fallback.json`。

## 演示得分点

| 标准 | 演示位置 |
|------|----------|
| 登录验证 | Login：留空、错误邮箱、短密码 |
| 错误处理 | 任意页状态栏红色错误信息；API 断网仍显示本地数据 |
| 暗色 / 字号 | Settings → Dark mode、Text size 滑块 → Save |
| 摇晃 | Home 页摇动设备 → 随机菜品 + 震动 |
| 震动 | 详情页 ♥ 收藏 |
| TTS | 详情页 **Read** |
| 摄像头 | Home **?** 旁 📷 或详情 📷 |
| 帮助导航 | Home **?** → Help，**←** 返回 |
| 数据来源 | Home 底部状态：`Local fallback` 或 `MockAPI` |

## Git 提交建议

1. **v1**：项目骨架、登录、首页列表、本地 JSON 兜底  
2. **v2**：MockAPI、收藏、详情、硬件功能完善  
3. **v3**：UI 打磨、README、部署截图与最终修复  

## 项目结构

- `Models/` — 数据模型  
- `Services/` — API、设置、收藏、硬件封装  
- `ViewModels/` — MVVM 视图模型  
- `Views/` — XAML 页面  
- `Constants/` — API 与偏好键  
