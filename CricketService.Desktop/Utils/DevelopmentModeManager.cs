namespace CricketService.Desktop.Utils
{
    public class DevelopmentModeManager
    {
        private readonly CheckBox chkDevMode;
        
        public DevelopmentModeManager(CheckBox chkDevMode)
        {
            this.chkDevMode = chkDevMode;
            InitializeDevelopmentMode();
        }
        
        public bool IsDevelopmentMode()
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
        
        public void OnCheckedChanged()
        {
            // Update environment variable when checkbox changes
            Environment.SetEnvironmentVariable("CRICKET_SERVICE_DEV", 
                chkDevMode.Checked ? "true" : "false", 
                EnvironmentVariableTarget.Process);
        }
        
        public string GetCurrentModeDescription()
        {
            return IsDevelopmentMode() ? "Development (Terminals Visible)" : "Production (Background)";
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
    }
}