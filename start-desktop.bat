@echo off
REM CricketService Desktop Launcher
REM Builds and runs the desktop UI application

echo.
echo 🖥️ Starting CricketService Desktop Launcher...
echo.

REM Check if the desktop project exists
if not exist "CricketService.Desktop\CricketService.Desktop.csproj" (
    echo ❌ Desktop project not found!
    echo Make sure CricketService.Desktop project exists.
    pause
    exit /b 1
)

REM Build the desktop application
echo 🔨 Building desktop application...
dotnet build CricketService.Desktop\CricketService.Desktop.csproj --configuration Release

if %ERRORLEVEL% neq 0 (
    echo ❌ Build failed!
    pause
    exit /b 1
)

REM Run the desktop application
echo 🚀 Launching desktop application...
dotnet run --project CricketService.Desktop --configuration Release

echo.
echo 👋 Desktop application closed.
pause