namespace CricketService.Desktop.UI
{
    public class StatusManager
    {
        private readonly Label lblStatus;
        private readonly ProgressBar progressBar;
        
        public StatusManager(Label lblStatus, ProgressBar progressBar)
        {
            this.lblStatus = lblStatus;
            this.progressBar = progressBar;
        }
        
        public void UpdateStatus(string message, bool showProgress = false)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(() => {
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
    }
}