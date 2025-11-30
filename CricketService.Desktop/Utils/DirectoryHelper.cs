namespace CricketService.Desktop.Utils
{
    public static class DirectoryHelper
    {
        public static string GetSolutionDirectory()
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
    }
}