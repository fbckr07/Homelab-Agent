using HomelabAgent.Logging.SimpleLogger;

namespace HomelabAgent.Web.Config;

public static class ConfigManager
{
    private const string ConfigDirectory = "./config/";
    private static readonly List<string> ConfigFiles = new() { "host.ini", "agent.ini" };
    private static List<string> MissingConfigFiles = new();
    
    
    public static async Task Initialize()
    {
        SimpleLogger.LogInformation("Checking config files...");
        await CheckConfigFiles();
    }

    public static async Task CheckConfigFiles()
    {
        CheckConfigDirectory();
        foreach (var file in ConfigFiles)
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