@echo off
REM CricketService - Start API Only Script (Windows)
REM This script starts the database and backend API only

REM Check for silent mode parameter
if "%1"=="--silent" (
    set SILENT_MODE=true
) else (
    set SILENT_MODE=false
)

if "%SILENT_MODE%"=="false" (
    echo.
    echo 🔧 Starting CricketService API Only...
    echo.
)

REM Start Docker containers for databases
if "%SILENT_MODE%"=="false" echo 📦 Starting PostgreSQL databases...
docker-compose up -d cricket-service-database hangfire-database >nul 2>&1

REM Wait for databases to be ready
if "%SILENT_MODE%"=="false" echo ⏳ Waiting for databases to initialize...
timeout /t 5 /nobreak >nul

REM Start backend API in new window
if "%SILENT_MODE%"=="false" (
    echo 🔧 Starting Backend API on port 5001...
    start "CricketService Backend API" cmd /k "cd CricketService.Api && dotnet run"
) else (
    start "" /min cmd /c "cd CricketService.Api && dotnet run >nul 2>&1"
)

REM Wait for backend to start
if "%SILENT_MODE%"=="false" echo ⏳ Waiting for backend to initialize...
timeout /t 5 /nobreak >nul

REM Open Swagger UI
if "%SILENT_MODE%"=="false" echo 🔧 Opening Backend Swagger UI in Chrome...
start chrome http://localhost:5001/swagger/index.html

if "%SILENT_MODE%"=="false" (
    echo.
    echo ✅ API service started successfully!
    echo.
    echo 📍 Services:
    echo    - Backend API: http://localhost:5001
    echo    - Swagger UI: http://localhost:5001/swagger
    echo    - PostgreSQL (Cricket DB): localhost:5440
    echo    - PostgreSQL (Hangfire DB): localhost:5441
    echo.
    echo Close the API window to stop the service.
    echo.
    pause
)