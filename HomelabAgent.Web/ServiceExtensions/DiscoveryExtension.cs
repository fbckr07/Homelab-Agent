using HomelabAgent.Net.HostDiscovery;

namespace HomelabAgent.Web.ServiceExtensions;

public static class DiscoveryExtension
{
    public static IServiceCollection AddUdpDiscovery(this IServiceCollection services)
    {
        services.AddHostedService<DiscoveryListener>();
        return services;
    }
}