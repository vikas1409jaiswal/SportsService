# CricketService Desktop Shortcut Creator
# Creates shortcuts for both the batch launcher and the desktop app

$WshShell = New-Object -ComObject WScript.Shell
$DesktopPath = [System.Environment]::GetFolderPath('Desktop')

# Create shortcut for the desktop launcher
$ShortcutPath = Join-Path $DesktopPath "CricketService Launcher.lnk"

$Shortcut = $WshShell.CreateShortcut($ShortcutPath)
$Shortcut.TargetPath = "dotnet.exe"
$Shortcut.Arguments = "run --project `"$PSScriptRoot\CricketService.Desktop`""
$Shortcut.WorkingDirectory = $PSScriptRoot
$Shortcut.Description = "CricketService Desktop Launcher - Choose how to start your services"
$Shortcut.IconLocation = "shell32.dll,137"  # Rocket icon
$Shortcut.WindowStyle = 1  # Normal window
$Shortcut.Save()

Write-Host "Desktop shortcut created successfully!" -ForegroundColor Green
Write-Host "Shortcut location: $ShortcutPath" -ForegroundColor Cyan
Write-Host ""
Write-Host "You can now double-click the shortcut on your desktop to launch the CricketService UI!" -ForegroundColor Yellow
