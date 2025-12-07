using System.Diagnostics;
using CricketService.Desktop.Services;
using CricketService.Desktop.UI;
using CricketService.Desktop.Utils;

namespace CricketService.Desktop
{
    public partial class MainForm : Form
    {
        private readonly ServiceManager serviceManager;
        private readonly ButtonManager buttonManager;
        private readonly StatusManager statusManager;
        private readonly DevelopmentModeManager developmentModeManager;
        private System.Windows.Forms.Timer statusTimer = new();

        public MainForm()
        {
            InitializeComponent();

            // Initialize managers
            var solutionDirectory = DirectoryHelper.GetSolutionDirectory();
            serviceManager = new ServiceManager(solutionDirectory);
            buttonManager = new ButtonManager(btnStartAll, btnStartFrontend, btnStartApi,
                                            btnStopAll, btnStopFrontend, btnStopApi);
            statusManager = new StatusManager(lblStatus, progressBar);
            developmentModeManager = new DevelopmentModeManager(chkDevMode);

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
        }

        private void InitializeServiceTracking()
        {
            // Setup status update timer
            statusTimer = new System.Windows.Forms.Timer();
            statusTimer.Interval = 2000; // Check every 2 seconds
            statusTimer.Tick += StatusTimer_Tick;
            statusTimer.Start();

            // Initialize UI
            buttonManager.InitializeStopButtons();
            UpdateServiceUI();
        }

        private void StatusTimer_Tick(object? sender, EventArgs e)
        {
            serviceManager.UpdateServiceStates();
            UpdateServiceUI();
        }

        private void UpdateServiceUI()
        {
            buttonManager.UpdateButtonStates(serviceManager.ServiceStates);
        }

        private void btnStartAll_Click(object sender, EventArgs e)
        {
            StartService("all", "Starting all services...", "All services started successfully!");
        }

        private void btnStartFrontend_Click(object sender, EventArgs e)
        {
            StartService("frontend", "Starting frontend only...", "Frontend started successfully!");
        }

        private void btnStartApi_Click(object sender, EventArgs e)
        {
            StartService("api", "Starting API only...", "API started successfully!");
        }

        private void StartService(string serviceName, string startingMessage, string successMessage)
        {
            try
            {
                statusManager.UpdateStatus(startingMessage);
                buttonManager.DisableAllStartButtons();

                serviceManager.StartService(serviceName, developmentModeManager.IsDevelopmentMode());

                statusManager.UpdateStatus(successMessage);
                UpdateServiceUI();

                // Re-enable buttons after a delay
                ScheduleButtonReEnable();
            }
            catch (Exception ex)
            {
                HandleServiceStartError(ex, serviceName);
            }
        }

        private void ScheduleButtonReEnable()
        {
            Task.Delay(2000).ContinueWith(t =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(buttonManager.EnableAllStartButtons);
                }
                else
                {
                    buttonManager.EnableAllStartButtons();
                }
            });
        }

        private void HandleServiceStartError(Exception ex, string serviceName)
        {
            MessageBox.Show($"Error starting {serviceName}: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
            buttonManager.EnableAllStartButtons();
            statusManager.UpdateStatus("Ready", false);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkDevMode_CheckedChanged(object sender, EventArgs e)
        {
            developmentModeManager.OnCheckedChanged();
            var mode = developmentModeManager.GetCurrentModeDescription();
            statusManager.UpdateStatus($"Mode: {mode}", false);
        }

        private void btnStopAll_Click(object sender, EventArgs e)
        {
            ExecuteStopAction("all", "Stopping all services...", "All services stopped successfully!");
        }

        private void btnStopFrontend_Click(object sender, EventArgs e)
        {
            ExecuteStopAction("frontend", "Stopping frontend...", "Frontend stopped successfully!");
        }

        private void btnStopApi_Click(object sender, EventArgs e)
        {
            ExecuteStopAction("api", "Stopping API...", "API stopped successfully!");
        }

        private void ExecuteStopAction(string serviceName, string stoppingMessage, string successMessage)
        {
            try
            {
                statusManager.UpdateStatus(stoppingMessage, true);
                serviceManager.StopService(serviceName);
                UpdateServiceUI();
                statusManager.UpdateStatus(successMessage, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping {serviceName}: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusManager.UpdateStatus("Ready", false);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            serviceManager.CleanupProcesses();
        }
    }
}