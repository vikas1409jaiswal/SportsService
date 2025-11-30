using System.Diagnostics;
using System.Linq;
using System.Management;

namespace CricketService.Desktop
{
    public partial class MainForm : Form
    {
        private Dictionary<string, List<Process>> serviceProcesses = new();
        private Dictionary<string, bool> serviceStates = new();
        private System.Windows.Forms.Timer statusTimer = new();
        
        public MainForm()
        {
            InitializeComponent();
            SetupForm();
            InitializeServiceTracking();
        }

        private void SetupForm()
        {
            // Set form properties
            this.Text = "CricketService Launcher";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(550, 350);
            this.MaximizeBox = true;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            
            // Set background gradient
            this.BackColor = Color.FromArgb(240, 248, 255);
            
            // Set icon
            try
            {
                this.Icon = SystemIcons.Application;
            }
            catch { /* Ignore icon errors */ }
            
            // Initialize development mode checkbox
            InitializeDevelopmentMode();
        }

        private void InitializeServiceTracking()
        {
            serviceProcesses = new Dictionary<string, List<Process>>
            {
                { "all", new List<Process>() },
                { "frontend", new List<Process>() },
                { "api", new List<Process>() }
            };
            
            serviceStates = new Dictionary<string, bool>
            {
                { "all", false },
                { "frontend", false },
                { "api", false }
            };
            
            // Setup status update timer
            statusTimer = new System.Windows.Forms.Timer();
            statusTimer.Interval = 2000; // Check every 2 seconds
            statusTimer.Tick += StatusTimer_Tick;
            statusTimer.Start();
            
            // Initialize UI after service tracking is set up
            InitializeStopButtons();
            UpdateServiceUI();
        }
        
        private void InitializeStopButtons()
        {
            // Initialize stop buttons as disabled with proper colors
            if (btnStopAll != null) 
            {
                btnStopAll.Enabled = false;
                btnStopAll.BackColor = Color.FromArgb(158, 158, 158);
            }
            if (btnStopFrontend != null) 
            {
                btnStopFrontend.Enabled = false;
                btnStopFrontend.BackColor = Color.FromArgb(158, 158, 158);
            }
            if (btnStopApi != null) 
            {
                btnStopApi.Enabled = false;
                btnStopApi.BackColor = Color.FromArgb(158, 158, 158);
            }
        }

        private void StatusTimer_Tick(object? sender, EventArgs e)
        {
            UpdateServiceStates();
            UpdateServiceUI();
        }

        private void UpdateServiceStates()
        {
            // Only check tracked processes, don't auto-detect external processes
            foreach (var service in serviceStates.Keys.ToList())
            {
                var processes = serviceProcesses[service];
                var runningProcesses = processes.Where(p => {
                    try 
                    { 
                        return !p.HasExited; 
                    } 
                    catch 
                    { 
                        return false; 
                    }
                }).ToList();
                
                // Update the tracked process list
                serviceProcesses[service] = runningProcesses;
                
                // Only mark as running if we have tracked processes that are still alive
                // Don't change state to false automatically (user controls this via stop button)
                if (runningProcesses.Any())
                {
                    serviceStates[service] = true;
                }
            }
        }

        private void UpdateServiceUI()
        {
            UpdateButtonStates("all", btnStartAll, btnStopAll);
            UpdateButtonStates("frontend", btnStartFrontend, btnStopFrontend);
            UpdateButtonStates("api", btnStartApi, btnStopApi);
        }

        private void UpdateButtonStates(string serviceName, Button startBtn, Button stopBtn)
        {
            bool isRunning = serviceStates[serviceName];
            
            startBtn.Enabled = !isRunning;
            stopBtn.Enabled = isRunning;
            
            // Update button text to show status
            if (isRunning)
            {
                // Only add status if not already there
                if (!startBtn.Text.Contains("●"))
                {
                    startBtn.Text = startBtn.Text + " - ● RUNNING";
                }
                startBtn.BackColor = Color.FromArgb(102, 187, 106); // Lighter green for running
                stopBtn.BackColor = Color.FromArgb(244, 67, 54); // Red for active stop
            }
            else
            {
                // Reset to original text without status
                if (startBtn.Text.Contains("●"))
                {
                    var statusIndex = startBtn.Text.IndexOf(" - ●");
                    if (statusIndex > 0)
                    {
                        startBtn.Text = startBtn.Text.Substring(0, statusIndex);
                    }
                }
                
                // Reset colors based on service type
                if (serviceName == "all")
                    startBtn.BackColor = Color.FromArgb(76, 175, 80);
                else if (serviceName == "frontend")
                    startBtn.BackColor = Color.FromArgb(33, 150, 243);
                else if (serviceName == "api")
                    startBtn.BackColor = Color.FromArgb(255, 152, 0);
                    
                stopBtn.BackColor = Color.FromArgb(158, 158, 158); // Gray when disabled
            }
        }

        private void btnStartAll_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Starting all services...");
                btnStartAll.Enabled = false;
                btnStartFrontend.Enabled = false;
                btnStartApi.Enabled = false;

                string solutionDir = GetSolutionDirectory();
                string batchFile = Path.Combine(solutionDir, "start-all.bat");

                if (!File.Exists(batchFile))
                {
                    MessageBox.Show($"Batch file not found: {batchFile}", "Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = batchFile,
                    Arguments = IsDevelopmentMode() ? "" : "--silent",
                    WorkingDirectory = solutionDir,
                    UseShellExecute = true,
                    CreateNoWindow = !IsDevelopmentMode(),
                    WindowStyle = IsDevelopmentMode() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
                };

                Process process = Process.Start(processInfo);
                if (process != null)
                {
                    serviceProcesses["all"].Add(process);
                }
                serviceStates["all"] = true;
                UpdateStatus("All services started successfully!");
                UpdateServiceUI();
                
                // Re-enable buttons after a delay
                Task.Delay(2000).ContinueWith(t =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(() =>
                        {
                            btnStartAll.Enabled = true;
                            btnStartFrontend.Enabled = true;
                            btnStartApi.Enabled = true;
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting all services: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartAll.Enabled = true;
                btnStartFrontend.Enabled = true;
                btnStartApi.Enabled = true;
                UpdateStatus("Ready", false);
            }
        }

        private void btnStartFrontend_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Starting frontend only...");
                btnStartAll.Enabled = false;
                btnStartFrontend.Enabled = false;
                btnStartApi.Enabled = false;

                string solutionDir = GetSolutionDirectory();
                string batchFile = Path.Combine(solutionDir, "start-frontend.bat");

                if (!File.Exists(batchFile))
                {
                    MessageBox.Show($"Batch file not found: {batchFile}", "Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = batchFile,
                    Arguments = IsDevelopmentMode() ? "" : "--silent",
                    WorkingDirectory = solutionDir,
                    UseShellExecute = true,
                    CreateNoWindow = !IsDevelopmentMode(),
                    WindowStyle = IsDevelopmentMode() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
                };

                Process process = Process.Start(processInfo);
                if (process != null)
                {
                    serviceProcesses["frontend"].Add(process);
                }
                serviceStates["frontend"] = true;
                UpdateStatus("Frontend started successfully!");
                UpdateServiceUI();
                
                // Re-enable buttons after a delay
                Task.Delay(2000).ContinueWith(t =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(() =>
                        {
                            btnStartAll.Enabled = true;
                            btnStartFrontend.Enabled = true;
                            btnStartApi.Enabled = true;
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting frontend: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartAll.Enabled = true;
                btnStartFrontend.Enabled = true;
                btnStartApi.Enabled = true;
                UpdateStatus("Ready", false);
            }
        }

        private void btnStartApi_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Starting API only...");
                btnStartAll.Enabled = false;
                btnStartFrontend.Enabled = false;
                btnStartApi.Enabled = false;

                string solutionDir = GetSolutionDirectory();
                string batchFile = Path.Combine(solutionDir, "start-api.bat");

                if (!File.Exists(batchFile))
                {
                    MessageBox.Show($"Batch file not found: {batchFile}", "Error", 
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = batchFile,
                    Arguments = IsDevelopmentMode() ? "" : "--silent",
                    WorkingDirectory = solutionDir,
                    UseShellExecute = true,
                    CreateNoWindow = !IsDevelopmentMode(),
                    WindowStyle = IsDevelopmentMode() ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden
                };

                Process process = Process.Start(processInfo);
                if (process != null)
                {
                    serviceProcesses["api"].Add(process);
                }
                serviceStates["api"] = true;
                UpdateStatus("API started successfully!");
                UpdateServiceUI();
                
                // Re-enable buttons after a delay
                Task.Delay(2000).ContinueWith(t =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(() =>
                        {
                            btnStartAll.Enabled = true;
                            btnStartFrontend.Enabled = true;
                            btnStartApi.Enabled = true;
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting API: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStartAll.Enabled = true;
                btnStartFrontend.Enabled = true;
                btnStartApi.Enabled = true;
                UpdateStatus("Ready", false);
            }
        }

        private string GetSolutionDirectory()
        {
            // Start from the application directory and look for the solution file
            string currentDir = Path.GetDirectoryName(Application.ExecutablePath) ?? Environment.CurrentDirectory;
            
            // Go up directories until we find the solution file
            DirectoryInfo? dir = new DirectoryInfo(currentDir);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "CricketService.sln")))
                {
                    return dir.FullName;
                }
                dir = dir.Parent;
            }
            
            // If solution file not found, return current directory
            return currentDir;
        }

        private void UpdateStatus(string message, bool showProgress = false)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => {
                    lblStatus.Text = $"Status: {message}";
                    progressBar.Visible = showProgress;
                });
            }
            else
            {
                lblStatus.Text = $"Status: {message}";
                progressBar.Visible = showProgress;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsDevelopmentMode()
        {
            // Check checkbox first, then environment variable
            if (chkDevMode != null)
            {
                return chkDevMode.Checked;
            }
            
            // Fallback to environment variable
            string? devMode = Environment.GetEnvironmentVariable("CRICKET_SERVICE_DEV");
            return !string.IsNullOrEmpty(devMode) && 
                   (devMode.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                    devMode.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                    devMode.Equals("development", StringComparison.OrdinalIgnoreCase));
        }

        private void InitializeDevelopmentMode()
        {
            // Set initial checkbox state based on environment variable
            bool envDevMode = false;
            string? devMode = Environment.GetEnvironmentVariable("CRICKET_SERVICE_DEV");
            if (!string.IsNullOrEmpty(devMode))
            {
                envDevMode = devMode.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                           devMode.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                           devMode.Equals("development", StringComparison.OrdinalIgnoreCase);
            }
            
            if (chkDevMode != null)
            {
                chkDevMode.Checked = envDevMode;
            }
        }

        private void chkDevMode_CheckedChanged(object sender, EventArgs e)
        {
            // Update environment variable when checkbox changes
            Environment.SetEnvironmentVariable("CRICKET_SERVICE_DEV", 
                chkDevMode.Checked ? "true" : "false", 
                EnvironmentVariableTarget.Process);
                
            // Update status to show current mode
            string mode = chkDevMode.Checked ? "Development (Terminals Visible)" : "Production (Background)";
            UpdateStatus($"Mode: {mode}", false);
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Stopping all services...", true);
                StopService("all");
                UpdateStatus("All services stopped successfully!", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping services: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Ready", false);
            }
        }

        private void btnStopFrontend_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Stopping frontend...", true);
                StopService("frontend");
                UpdateStatus("Frontend stopped successfully!", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping frontend: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Ready", false);
            }
        }

        private void btnStopApi_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("Stopping API...", true);
                StopService("api");
                UpdateStatus("API stopped successfully!", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping API: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Ready", false);
            }
        }

        private void StopService(string serviceName)
        {
            // Stop tracked processes
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
            
            // Kill related processes by name
            try
            {
                var allProcesses = Process.GetProcesses();
                switch (serviceName)
                {
                    case "all":
                        KillProcessesByPattern(allProcesses, "CricketService");
                        KillProcessesByPattern(allProcesses, "node");
                        KillProcessesByPattern(allProcesses, "dotnet");
                        KillProcessesByPattern(allProcesses, "chrome");
                        KillDockerContainers();
                        // Mark all services as stopped
                        serviceStates["all"] = false;
                        serviceStates["frontend"] = false;
                        serviceStates["api"] = false;
                        break;
                    case "frontend":
                        KillProcessesByPattern(allProcesses, "node");
                        KillProcessesByPattern(allProcesses, "chrome");
                        serviceStates["frontend"] = false;
                        break;
                    case "api":
                        KillProcessesByPattern(allProcesses, "dotnet");
                        KillDockerContainers();
                        serviceStates["api"] = false;
                        break;
                }
            }
            catch { /* Ignore errors */ }
            
            // Update the UI to reflect the stopped state
            UpdateServiceUI();
        }

        private void KillProcessesByPattern(Process[] processes, string pattern)
        {
            // List of processes to avoid killing
            var protectedProcesses = new[] { "code", "devenv", "notepad", "explorer", "winlogon", "csrss", "dwm", "winlogon" };
            
            foreach (var process in processes)
            {
                try
                {
                    // Skip protected processes
                    if (protectedProcesses.Any(p => process.ProcessName.ToLower().Contains(p)))
                        continue;
                        
                    // For specific patterns, be more targeted
                    bool shouldKill = false;
                    
                    if (pattern.ToLower() == "node")
                    {
                        // Only kill Node.js processes with CricketService in window title or command line
                        shouldKill = process.ProcessName.ToLower().Equals("node") &&
                                   (!string.IsNullOrEmpty(process.MainWindowTitle) && 
                                    (process.MainWindowTitle.ToLower().Contains("cricket") ||
                                     process.MainWindowTitle.ToLower().Contains("localhost:3000")));
                    }
                    else if (pattern.ToLower() == "dotnet")
                    {
                        // Only kill .NET processes that are likely CricketService related
                        shouldKill = process.ProcessName.ToLower().Equals("dotnet") &&
                                   (!string.IsNullOrEmpty(process.MainWindowTitle) && 
                                    (process.MainWindowTitle.ToLower().Contains("cricket") ||
                                     process.MainWindowTitle.ToLower().Contains("cricketservice")));
                    }
                    else if (pattern.ToLower() == "chrome")
                    {
                        // Only kill Chrome processes with specific user data directory for CricketService
                        shouldKill = process.ProcessName.ToLower().Contains("chrome") &&
                                   (!string.IsNullOrEmpty(process.MainWindowTitle) && 
                                    (process.MainWindowTitle.ToLower().Contains("localhost:3000") ||
                                     process.MainWindowTitle.ToLower().Contains("cricketservice")));
                        
                        // Additional check: try to get command line to see if it uses our specific user-data-dir
                        if (!shouldKill)
                        {
                            try
                            {
                                // This is a safer approach - only kill if it's our specific Chrome instance
                                var commandLine = GetProcessCommandLine(process);
                                shouldKill = commandLine?.Contains("CricketService-Chrome") == true;
                            }
                            catch { /* Ignore command line check errors */ }
                        }
                    }
                    else
                    {
                        // For other patterns, check both process name and window title but be more specific
                        shouldKill = (process.ProcessName.ToLower().Contains(pattern.ToLower()) ||
                                     (!string.IsNullOrEmpty(process.MainWindowTitle) && 
                                      process.MainWindowTitle.ToLower().Contains(pattern.ToLower()))) &&
                                     (!string.IsNullOrEmpty(process.MainWindowTitle) && 
                                      (process.MainWindowTitle.ToLower().Contains("cricket") ||
                                       process.MainWindowTitle.ToLower().Contains("localhost")));
                    }
                    
                    if (shouldKill)
                    {
                        process.Kill();
                    }
                }
                catch { /* Ignore errors */ }
            }
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

        private void KillProcessesByPattern(Process[] processes, string pattern, string? processType = null)
        {
            foreach (var process in processes)
            {
                try
                {
                    if (processType != null)
                    {
                        // Kill all processes of the specified type
                        if (process.ProcessName.ToLower().Contains(processType.ToLower()))
                        {
                            process.Kill();
                        }
                    }
                    else
                    {
                        // Kill processes matching the pattern
                        if (process.MainWindowTitle.Contains(pattern) || 
                            process.ProcessName.Contains(pattern))
                        {
                            process.Kill();
                        }
                    }
                }
                catch { /* Ignore errors */ }
            }
        }

        private void KillDockerContainers()
        {
            try
            {
                var dockerStop = new ProcessStartInfo
                {
                    FileName = "docker-compose",
                    Arguments = "down",
                    WorkingDirectory = GetSolutionDirectory(),
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                Process.Start(dockerStop)?.WaitForExit(10000);
            }
            catch { /* Ignore docker errors */ }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Clean up any running processes
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
    }
}