namespace HomelabAgent.Web.Config;

public static class FileContents
{
    private static string HostConfigContent = """
                                              [Host]
                                              HostAdress=1.1.1.1,
                                              HostPort=33655
                                              """;
    
    
    public static Dictionary<string, string> ConfigFileContents = new()
    {
        { "host.ini", HostConfigContent },
        { "agent.ini", string.Empty}
    };
}