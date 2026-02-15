# WM Office (Windows)

Native Windows application built with .NET 6+ and WinUI 3.

## Architecture
- **Driven by:** `wm-master-spec` (The Brain)
- **Core Logic:** `CableSizingService.cs` implements standardized engineering calculations.
- **Data Sync:** `SyncQueue.cs` manages robust offline synchronization.

## Setup
1. Open `WMOffice.sln` in Visual Studio 2022.
2. Ensure "Windows App SDK" workload is installed.
3. Build & Run (F5).

## Requirements
- Visual Studio 2022
- .NET 6 SDK or later
