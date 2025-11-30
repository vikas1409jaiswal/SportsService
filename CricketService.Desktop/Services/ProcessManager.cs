using System.Diagnostics;
using System.Management;
using CricketService.Desktop.Services;

namespace CricketService.Desktop.Services
{
    public class ProcessManager
    {
        public void KillProcessesByPattern(Process[] processes, string pattern)
        {
            foreach (var process in processes)
            {
                try
                {
                    if (ShouldProtectProcess(process))
                        continue;
                        
                    if (ShouldKillProcess(process, pattern))
                    {
                        process.Kill();
                    }
                }
                catch { /* Ignore errors */ }
            }
        }
        
        private bool ShouldProtectProcess(Process process)
        {
            return ServiceConfiguration.ProtectedProcesses
                .Any(p => process.ProcessName.ToLower().Contains(p));
        }
        
        private bool ShouldKillProcess(Process process, string pattern)
        {
            return pattern.ToLower() switch
            {
                "node" => IsTargetNodeProcess(process),
                "dotnet" => IsTargetDotNetProcess(process),
                "chrome" => IsTargetChromeProcess(process),
                _ => IsGenericTargetProcess(process, pattern)
            };
        }
        
        private bool IsTargetNodeProcess(Process process)
        {
            return process.ProcessName.ToLower().Equals("node") &&
                   HasCricketServiceInTitle(process);
        }
        
        private bool IsTargetDotNetProcess(Process process)
        {
            return process.ProcessName.ToLower().Equals("dotnet") &&
                   HasCricketServiceInTitle(process);
        }
        
        private bool IsTargetChromeProcess(Process process)
        {
            if (!process.ProcessName.ToLower().Contains("chrome"))
                return false;
                
            // Check window title for localhost or cricketservice
            if (HasLocalhostOrCricketServiceInTitle(process))
                return true;
                
            // Check command line for our specific user data directory
            try
            {
                var commandLine = GetProcessCommandLine(process);
                return commandLine?.Contains("CricketService-Chrome") == true;
            }
            catch
            {
                return false;
            }
        }
        
        private bool IsGenericTargetProcess(Process process, string pattern)
        {
            var hasPatternInName = process.ProcessName.ToLower().Contains(pattern.ToLower());
            var hasPatternInTitle = !string.IsNullOrEmpty(process.MainWindowTitle) && 
                                   process.MainWindowTitle.ToLower().Contains(pattern.ToLower());
            var hasCricketServiceContext = HasCricketServiceInTitle(process) || 
                                          HasLocalhostInTitle(process);
            
            return (hasPatternInName || hasPatternInTitle) && hasCricketServiceContext;
        }
        
        private bool HasCricketServiceInTitle(Process process)
        {
            return !string.IsNullOrEmpty(process.MainWindowTitle) && 
                   (process.MainWindowTitle.ToLower().Contains("cricket") ||
                    process.MainWindowTitle.ToLower().Contains("localhost:3000"));
        }
        
        private bool HasLocalhostOrCricketServiceInTitle(Process process)
        {
            return !string.IsNullOrEmpty(process.MainWindowTitle) && 
                   (process.MainWindowTitle.ToLower().Contains("localhost:3000") ||
                    process.MainWindowTitle.ToLower().Contains("cricketservice"));
        }
        
        private bool HasLocalhostInTitle(Process process)
        {
            return !string.IsNullOrEmpty(process.MainWindowTitle) && 
                   process.MainWindowTitle.ToLower().Contains("localhost");
        }
        
        private string? GetProcessCommandLine(Process process)
        {
            try
            {
                using (var searcher = new System.Management.ManagementObjectSearcher(
                    $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"))
                {
                    using (var objects = searcher.Get())
                    {
                        return objects.Cast<System.Management.ManagementBaseObject>()
                                      .SingleOrDefault()?["CommandLine"]?.ToString();
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        
        public void KillDockerContainers(string solutionDirectory)
        {
            try
            {
                var dockerStop = new ProcessStartInfo
                {
                    FileName = "docker-compose",
                    Arguments = "down",
                    WorkingDirectory = solutionDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process.Start(dockerStop)?.WaitForExit(10000);
            }
            catch { /* Ignore docker errors */ }
        }
    }
}