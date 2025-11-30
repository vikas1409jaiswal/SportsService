# CricketService Desktop Launcher

A simple Windows Forms desktop application that provides an easy way to start your CricketService application components.

## Features

- **🚀 Start All Services**: Launches all services including PostgreSQL databases, backend API, and frontend
- **🎨 Start Frontend Only**: Launches only the React frontend application
- **Simple UI**: Clean and intuitive interface with status updates
- **Error Handling**: Displays helpful error messages if batch files are not found

## How to Use

### Option 1: Using the Launcher Script
1. Run `start-desktop.bat` from the solution root
2. This will build and launch the desktop application
3. Choose your desired option from the dialog

### Option 2: Creating a Desktop Shortcut
1. Run the PowerShell script: `.\create-desktop-shortcut.ps1`
2. This will create a desktop shortcut for easy access
3. Double-click the shortcut to launch the application

### Option 3: Building Manually
```bash
# Build the application
dotnet build CricketService.Desktop\CricketService.Desktop.csproj

# Run the application
dotnet run --project CricketService.Desktop
```

## What It Does

### Start All Services Button
Executes `start-all.bat` which:
- Starts PostgreSQL databases via Docker Compose
- Launches the .NET Core backend API
- Starts the React frontend
- Opens browser windows for both Swagger UI and the frontend

### Start Frontend Only Button
Executes `start-frontend.bat` which:
- Launches only the React frontend application
- Opens the frontend in your default browser

## Requirements

- .NET 6.0 or higher
- Windows OS (Windows Forms application)
- All dependencies required by the main CricketService application

## Project Structure

- `MainForm.cs` - Main application logic and event handlers
- `MainForm.Designer.cs` - Windows Forms designer code
- `Program.cs` - Application entry point
- `CricketService.Desktop.csproj` - Project file

## How It Works

The application automatically locates your solution directory by searching upward from its execution location for the `CricketService.sln` file. It then executes the appropriate batch files from that directory, ensuring the correct working directory is used.

## Troubleshooting

- **"Batch file not found" error**: Ensure the `start-all.bat` and `start-frontend.bat` files exist in your solution root
- **Application won't start**: Make sure you have .NET 6.0 Windows Desktop Runtime installed
- **Services don't start**: Check that Docker Desktop is running (for database services) and all dependencies are installed