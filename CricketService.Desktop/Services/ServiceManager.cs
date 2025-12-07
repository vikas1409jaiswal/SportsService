using System.Diagnostics;
using CricketService.Desktop.Services;

namespace CricketService.Desktop.Services
{
    public class ServiceManager
    {
        private readonly ProcessManager processManager;
        private readonly Dictionary<string, List<Process>> serviceProcesses;
        private readonly Dictionary<string, bool> serviceStates;
        private readonly string solutionDirectory;

        public ServiceManager(string solutionDirectory)
        {
            this.solutionDirectory = solutionDirectory;
            this.processManager = new ProcessManager();
            this.serviceProcesses = new Dictionary<string, List<Process>>
            {
                { "all", new List<Process>() },
                { "frontend", new List<Process>() },
                { "api", new List<Process>() }
            };
            this.serviceStates = new Dictionary<string, bool>
            {
                { "all", false },
                { "frontend", false },
                { "api", false }
            };
        }

        public Dictionary<string, bool> ServiceStates => serviceStates;

        public void StartService(string serviceName, bool isDevelopmentMode)
        {
            var batchFileName = ServiceConfiguration.ServiceBatchFiles[serviceName];
            var batchFile = Path.Combine(solutionDirectory, batchFileName);

            if (!File.Exists(batchFile))
            {
                throw new FileNotFoundException($"Batch file not found: {batchFile}");
            }

            var processInfo = new ProcessStartInfo
            {
                FileName = batchFile,
                Arguments = isDevelopmentMode ? "" : "--silent",
                WorkingDirectory = solutionDirectory,
                UseShellExecute = true,
                CreateNoWindow = !isDevelopmentMode,
                WindowStyle = isDevelopmentMode ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
            };

            var process = Process.Start(processInfo);
            if (process != null)
            {
                serviceProcesses[serviceName].Add(process);
            }

            serviceStates[serviceName] = true;
        }

        public void StopService(string serviceName)
        {
            // Stop tracked processes
            StopTrackedProcesses(serviceName);

            // Kill related processes by name
            KillServiceProcesses(serviceName);
        }

        public void UpdateServiceStates()
        {
            foreach (var service in serviceStates.Keys.ToList())
            {
                var processes = serviceProcesses[service];
                var runningProcesses = processes.Where(p =>
                {
                    try
                    {
                        return !p.HasExited;
                    }
                    catch
                    {
                        return false;
                    }
                }).ToList();

                serviceProcesses[service] = runningProcesses;

                if (runningProcesses.Any())
                {
                    serviceStates[service] = true;
                }
            }
        }

        public void CleanupProcesses()
        {
            foreach (var serviceProcessList in serviceProcesses.Values)
            {
                foreach (var process in serviceProcessList.ToList())
                {
                    try
                    {
                        if (!process.HasExited)
                        {
                            process.Kill();
                        }
                        process.Dispose();
                    }
                    catch { /* Ignore cleanup errors */ }
                }
            }
        }

        private void StopTrackedProcesses(string serviceName)
        {
            var processes = serviceProcesses[serviceName];
            foreach (var process in processes.ToList())
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch { /* Ignore errors when killing processes */ }
            }
            processes.Clear();
        }

        private void KillServiceProcesses(string serviceName)
        {
            try
            {
                var allProcesses = Process.GetProcesses();
                var serviceConfig = GetServiceStopConfig(serviceName);

                foreach (var processPattern in serviceConfig.ProcessesToKill)
                {
                    processManager.KillProcessesByPattern(allProcesses, processPattern);
                }

                if (serviceConfig.KillDocker)
                {
                    processManager.KillDockerContainers(solutionDirectory);
                }

                foreach (var stateToUpdate in serviceConfig.StatesToUpdate)
                {
                    serviceStates[stateToUpdate] = false;
                }
            }
            catch { /* Ignore errors */ }
        }

        private ServiceStopConfig GetServiceStopConfig(string serviceName)
        {
            return serviceName switch
            {
                "all" => new ServiceStopConfig
                {
                    ProcessesToKill = new[] { "CricketService", "node", "dotnet", "chrome" },
                    KillDocker = true,
                    StatesToUpdate = new[] { "all", "frontend", "api" }
                },
                "frontend" => new ServiceStopConfig
                {
                    ProcessesToKill = new[] { "node", "chrome" },
                    KillDocker = false,
                    StatesToUpdate = new[] { "frontend" }
                },
                "api" => new ServiceStopConfig
                {
                    ProcessesToKill = new[] { "dotnet" },
                    KillDocker = true,
                    StatesToUpdate = new[] { "api" }
                },
                _ => throw new ArgumentException($"Unknown service: {serviceName}")
            };
        }
    }
}