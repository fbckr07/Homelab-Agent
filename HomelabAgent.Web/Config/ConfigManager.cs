using HomelabAgent.Logging.SimpleLogger;
using Microsoft.OpenApi;

namespace HomelabAgent.Web.Config;

public static class ConfigManager
{
    private static readonly string ConfigDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
    
    public static async Task Initialize()
    {
        SimpleLogger.LogInformation("Checking config files...");
        await CheckConfigFiles();
    }

    public static async Task CheckConfigFiles()
    {
        CheckConfigDirectory();
        
        List<string> MissingConfigFiles = new();

        var expectedFiles = FileContents.ConfigFileContents.Keys;
        
        foreach (var file in expectedFiles)
        {
            if (!File.Exists(Path.Combine(ConfigDirectory, file)))
            {
                SimpleLogger.LogError($"Config file {file} does not exist.");
                MissingConfigFiles.Add(file);
            }
        }

        foreach (var file in MissingConfigFiles)
        {
            try
            {
                await File.WriteAllTextAsync(
                    Path.Combine(ConfigDirectory, file),
                    FileContents.ConfigFileContents[file]);   
            }
            catch (Exception e)
            {
                SimpleLogger.LogError($"Failed to create config file {file}: {e.Message}", true);
            }
        }
    }

    private static void CheckConfigDirectory()
    {
        if (!Directory.Exists(ConfigDirectory))
        {
            try
            {
                Directory.CreateDirectory(ConfigDirectory);
            }
            catch (IOException e)
            {
                SimpleLogger.LogError($"Failed to create config directory: {e.Message}", true);
            }
            catch (UnauthorizedAccessException e)
            {
                SimpleLogger.LogError("Insufficient permissions. Failed to create config directory.", true);
            }
            catch (Exception e)
            {
                SimpleLogger.LogError($"Unexpected error while creating config directory: {e.Message}", true);
            }
        }
    }
}