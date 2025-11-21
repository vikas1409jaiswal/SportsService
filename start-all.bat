@echo off
REM CricketService - Start All Services Script (Windows)
REM This script starts the databases, backend API, and frontend

echo.
echo 🚀 Starting CricketService Application...
echo.

REM Start Docker containers for databases
echo 📦 Starting PostgreSQL databases...
docker-compose up -d cricket-service-database hangfire-database

REM Wait for databases to be ready
echo ⏳ Waiting for databases to initialize...
timeout /t 5 /nobreak >nul

REM Start backend API in new window
echo 🔧 Starting Backend API on port 5001...
start "CricketService Backend API" cmd /k "cd CricketService.Api && dotnet run"

REM Wait for backend to start
echo ⏳ Waiting for backend to initialize...
timeout /t 5 /nobreak >nul

REM Start frontend in new window
echo 🎨 Starting Frontend...
start "CricketService Frontend" cmd /k "cd CricketService.UI && npm run start-both"

REM Wait for frontend to start and then open in Chrome
echo ⏳ Waiting for frontend to initialize...
timeout /t 10 /nobreak >nul

REM Open Swagger UI first
echo 🔧 Opening Backend Swagger UI in Chrome...
start chrome http://localhost:5001/swagger/index.html

REM Wait a moment and then open frontend UI in new tab
timeout /t 2 /nobreak >nul
echo 🌐 Opening frontend in new tab...
start chrome http://localhost:44440/players-comparison

echo.
echo ✅ All services started successfully!
echo.
echo 📍 Services:
echo    - Backend API: http://localhost:5001
echo    - Frontend: http://localhost:44440
echo    - PostgreSQL (Cricket DB): localhost:5440
echo    - PostgreSQL (Hangfire DB): localhost:5441
echo.
echo Services are running in separate windows.
echo Close the windows to stop the services.
echo.
pause
