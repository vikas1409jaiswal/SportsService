using CricketService.Desktop.Services;

namespace CricketService.Desktop.UI
{
    public class ButtonManager
    {
        private readonly Button btnStartAll;
        private readonly Button btnStartFrontend;
        private readonly Button btnStartApi;
        private readonly Button btnStopAll;
        private readonly Button btnStopFrontend;
        private readonly Button btnStopApi;
        
        public ButtonManager(Button btnStartAll, Button btnStartFrontend, Button btnStartApi,
                           Button btnStopAll, Button btnStopFrontend, Button btnStopApi)
        {
            this.btnStartAll = btnStartAll;
            this.btnStartFrontend = btnStartFrontend;
            this.btnStartApi = btnStartApi;
            this.btnStopAll = btnStopAll;
            this.btnStopFrontend = btnStopFrontend;
            this.btnStopApi = btnStopApi;
        }
        
        public void InitializeStopButtons()
        {
            SetButtonState(btnStopAll, false, ServiceConfiguration.DisabledButtonColor);
            SetButtonState(btnStopFrontend, false, ServiceConfiguration.DisabledButtonColor);
            SetButtonState(btnStopApi, false, ServiceConfiguration.DisabledButtonColor);
        }
        
        public void DisableAllStartButtons()
        {
            btnStartAll.Enabled = false;
            btnStartFrontend.Enabled = false;
            btnStartApi.Enabled = false;
        }
        
        public void EnableAllStartButtons()
        {
            btnStartAll.Enabled = true;
            btnStartFrontend.Enabled = true;
            btnStartApi.Enabled = true;
        }
        
        public void UpdateButtonStates(Dictionary<string, bool> serviceStates)
        {
            UpdateServiceButtonStates("all", btnStartAll, btnStopAll, serviceStates["all"]);
            UpdateServiceButtonStates("frontend", btnStartFrontend, btnStopFrontend, serviceStates["frontend"]);
            UpdateServiceButtonStates("api", btnStartApi, btnStopApi, serviceStates["api"]);
        }
        
        private void UpdateServiceButtonStates(string serviceName, Button startBtn, Button stopBtn, bool isRunning)
        {
            startBtn.Enabled = !isRunning;
            stopBtn.Enabled = isRunning;
            
            if (isRunning)
            {
                SetRunningButtonState(startBtn, stopBtn);
            }
            else
            {
                SetStoppedButtonState(serviceName, startBtn, stopBtn);
            }
        }
        
        private void SetRunningButtonState(Button startBtn, Button stopBtn)
        {
            if (!startBtn.Text.Contains("●"))
            {
                startBtn.Text = startBtn.Text + " - ● RUNNING";
            }
            startBtn.BackColor = ServiceConfiguration.RunningServiceColor;
            stopBtn.BackColor = ServiceConfiguration.StopButtonActiveColor;
        }
        
        private void SetStoppedButtonState(string serviceName, Button startBtn, Button stopBtn)
        {
            RemoveRunningStatus(startBtn);
            startBtn.BackColor = ServiceConfiguration.ServiceColors.GetValueOrDefault(serviceName, ServiceConfiguration.AllServiceColor);
            stopBtn.BackColor = ServiceConfiguration.DisabledButtonColor;
        }
        
        private void RemoveRunningStatus(Button startBtn)
        {
            if (startBtn.Text.Contains("●"))
            {
                var statusIndex = startBtn.Text.IndexOf(" - ●");
                if (statusIndex > 0)
                {
                    startBtn.Text = startBtn.Text.Substring(0, statusIndex);
                }
            }
        }
        
        private void SetButtonState(Button button, bool enabled, Color color)
        {
            button.Enabled = enabled;
            button.BackColor = color;
        }
    }
}