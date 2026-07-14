namespace HomelabAgent.Logging.SimpleLogger;

public static class SimpleLogger
{
    public static void LogInformation(string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[INFO] [{timestamp}]  {message}");
        Console.ResetColor();
    }
    
    public static void LogWarning(string message)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARN] [{timestamp}]  {message}");
        Console.ResetColor();
    }

    public static void LogError(string message, bool ExitApplication = false)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] [{timestamp}]  {message}");
        Console.ResetColor();
        
        if (ExitApplication)
            Environment.Exit(1);
    }
    
    public static void LogException(Exception exception, bool ExitApplication = false)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[EXCEPTION] [{timestamp}]  {exception.Message}");
        Console.ResetColor();
        
        if (ExitApplication)
            Environment.Exit(1);
    }
}