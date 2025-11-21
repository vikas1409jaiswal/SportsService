# PowerShell script to create a desktop shortcut for start-all.bat

$WshShell = New-Object -ComObject WScript.Shell
$DesktopPath = [System.Environment]::GetFolderPath('Desktop')
$ShortcutPath = Join-Path $DesktopPath "Start CricketService.lnk"

$Shortcut = $WshShell.CreateShortcut($ShortcutPath)
$Shortcut.TargetPath = "cmd.exe"
$Shortcut.Arguments = "/c `"$PSScriptRoot\start-all.bat`""
$Shortcut.WorkingDirectory = $PSScriptRoot
$Shortcut.Description = "Start CricketService Application (Backend + Frontend + Databases)"
$Shortcut.IconLocation = "shell32.dll,137"  # Rocket icon
$Shortcut.Save()

Write-Host "Desktop shortcut created successfully!" -ForegroundColor Green
Write-Host "Shortcut location: $ShortcutPath" -ForegroundColor Cyan
Write-Host "Double-click the shortcut to start CricketService!" -ForegroundColor Yellow
