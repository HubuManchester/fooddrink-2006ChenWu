# World Cuisine Explorer (.NET MAUI)

World Cuisine Information Application

## Run

> **Note**: The project must be built within the `WorldCuisineApp` directory, or the `WorldCuisine.sln` file in the parent directory should be opened.  

### Environment

- .NET 9 SDK + MAUI workload
- Visual Studio 2022(Including the "Mobile Development Using .NET" workload) or `dotnet workload install maui`

### Windows

```powershell
cd WorldCuisineApp
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

Or, in Visual Studio, select **Windows Machine** and then press F5.

### Android

After connecting the device or simulator:

```powershell
dotnet build -f net9.0-android
dotnet build -t:Run -f net9.0-android
```

## MockAPI

1. Create a project on [mockapi.io](https://mockapi.io) and add the resource **`cuisines`** and **` user `**.
2. Sample fields: **`cuisines`**: `id`, `name`, `country`, `region`, `description`, `imageUrl`, `spiceLevel`, `funFact`. **` user `**: `username`,`email`, `password`(consistent with `Models/CuisineItem.cs`).
3. Paste the complete URL in the **Settings** of the application, for example: `https://6a1a6614bc2f944754922936.mockapi.io/:endpoint`
4. When the configuration is not set or the network fails, automatically use `Resources/Raw/cuisines_fallback.json`. 

## Presentation Evaluation Points 
| Standard | Demonstration Position | 
|----------|------------------------|
| Login Verification | Login & Register: Leave blank, incorrect email, short password |
| Error Handling | Red error message in the status bar of any page; Local data is displayed even when API is disconnected |
| Dark Mode / Font Size | Settings → Dark mode, Text size slider → Save |
| Shaking | Shake the device on the Home page → Random dishes + vibration |
| Vibration | On the Detail page ♥ Like |
| TTS | On the Detail page **Read** |
| Camera | Home **?** → 📷 or Detail 📷 |
| Help Navigation | Home **?** → Help, **←** Back |
| Data Source | Bottom status of Home: `Local fallback` or `MockAPI` | 

## Git Commit Suggestions 
1. **v1**: Project framework, login, home page list, local JSON fallback
2. **v2**: MockAPI, favorites, details, hardware functionality enhancement
3. **v3**: UI polishing, README, deployment screenshots and final fixes 

## Project Structure 
- `Models/` — Data Models
- `Services/` — APIs, settings, collections, hardware encapsulation
- `Views/` — XAML Pages
- `Constants/` — APIs and preference keys
- `Models/` — Data Models (CuisineItem, User)
- `Services/` — Business logic & data access (API, auth, favorites, hardware, settings)
- `ViewModels/` — MVVM ViewModels with data binding & commands
- `Views/` — XAML Pages (Login, Register, Home, Detail, Favorites, Settings, Camera, Help)
- `Converters/` — XAML value converters (bool invert, string not empty)
- `Constants/` — App-wide constants (default API URL, preference keys, shake thresholds)
- `Resources/Styles/` — Color palette, control styles, custom app styles
- `Resources/Raw/` — Offline fallback data (14 cuisines)
- `Platforms/` — Platform-specific entry points & manifests (Android, Windows)
- `AppShell.xaml` — Shell navigation (Login route + tab bar: Home/Favorites/Settings)
- `MauiProgram.cs` — Dependency injection registration
- `App.xaml` — Application root & resource dictionary merge