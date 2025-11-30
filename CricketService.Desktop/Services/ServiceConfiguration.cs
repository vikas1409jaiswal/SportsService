using System.Drawing;

namespace CricketService.Desktop.Services
{
    public static class ServiceConfiguration
    {
        // Service color constants
        public static readonly Color AllServiceColor = Color.FromArgb(76, 175, 80);
        public static readonly Color FrontendServiceColor = Color.FromArgb(33, 150, 243);
        public static readonly Color ApiServiceColor = Color.FromArgb(255, 152, 0);
        public static readonly Color RunningServiceColor = Color.FromArgb(102, 187, 106);
        public static readonly Color StopButtonActiveColor = Color.FromArgb(244, 67, 54);
        public static readonly Color DisabledButtonColor = Color.FromArgb(158, 158, 158);
        
        // Service configuration
        public static readonly Dictionary<string, string> ServiceBatchFiles = new()
        {
            { "all", "start-all.bat" },
            { "frontend", "start-frontend.bat" },
            { "api", "start-api.bat" }
        };
        
        public static readonly Dictionary<string, Color> ServiceColors = new()
        {
            { "all", AllServiceColor },
            { "frontend", FrontendServiceColor },
            { "api", ApiServiceColor }
        };
        
        public static readonly string[] ProtectedProcesses = 
        {
            "code", "devenv", "notepad", "explorer", "winlogon", "csrss", "dwm"
        };
    }
    
    public class ServiceStopConfig
    {
        public string[] ProcessesToKill { get; set; } = Array.Empty<string>();
        public bool KillDocker { get; set; }
        public string[] StatesToUpdate { get; set; } = Array.Empty<string>();
    }
}