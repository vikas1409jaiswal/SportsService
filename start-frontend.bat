@echo off
REM CricketService - Start UI Script (Windows)
REM This script starts the frontend

REM Check for silent mode parameter
if "%1"=="--silent" (
    set SILENT_MODE=true
) else (
    set SILENT_MODE=false
)

REM Start frontend in new window
if "%SILENT_MODE%"=="false" (
    echo 🎨 Starting Frontend...
    start "CricketService Frontend" cmd /k "cd CricketService.UI && npm run start-both"
) else (
    start "" /min cmd /c "cd CricketService.UI && npm run start-both >nul 2>&1"
)

REM Wait for frontend to start and then open in Chrome
if "%SILENT_MODE%"=="false" echo ⏳ Waiting for frontend to initialize...
timeout /t 10 /nobreak >nul

REM Wait a moment and then open frontend UI in new tab
timeout /t 2 /nobreak >nul
if "%SILENT_MODE%"=="false" echo 🌐 Opening frontend in new tab...
start "" "chrome.exe" --incognito --new-window "http://localhost:44440/records" --user-data-dir="%TEMP%\CricketService-Chrome"

if "%SILENT_MODE%"=="false" (
    echo.
    echo ✅ All services started successfully!
    echo.
    echo 📍 Services:
    echo    - Frontend: http://localhost:44440
    echo.
    echo Close the windows to stop the services.
    echo.
    pause
)
