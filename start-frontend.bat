@echo off
REM CricketService - Start UI Script (Windows)
REM This script starts the frontend

REM Start frontend in new window
echo 🎨 Starting Frontend...
start "CricketService Frontend" cmd /k "cd CricketService.UI && npm run start-both"

REM Wait for frontend to start and then open in Chrome
echo ⏳ Waiting for frontend to initialize...
timeout /t 10 /nobreak >nul

REM Wait a moment and then open frontend UI in new tab
timeout /t 2 /nobreak >nul
echo 🌐 Opening frontend in new tab...
start chrome http://localhost:44440/records

echo.
echo ✅ All services started successfully!
echo.
echo 📍 Services:
echo    - Frontend: http://localhost:44440
echo.
echo Close the windows to stop the services.
echo.
pause
