using System.Net.Sockets;
using System.Text;
using HomelabAgent.Logging.SimpleLogger;
using Microsoft.Extensions.Hosting;

namespace HomelabAgent.Net.HostDiscovery;

public class DiscoveryListener : BackgroundService
{
    private readonly string ExpectedRequest = "HOMELAB_SERVICE_DISCOVERY_REQUEST";
    private readonly string ResponseMessage = "HOMELAB_SERVICE_DISCOVERY_ALIVE";
    private UdpClient?  udpClient;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        udpClient = new UdpClient(30766);
        SimpleLogger.LogInformation("Discovery listener started on UDP port 30766.");
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var receivedResult = await udpClient.ReceiveAsync(cancellationToken);
                string requestText = Encoding.UTF8.GetString(receivedResult.Buffer);
                
                SimpleLogger.LogInformation($"Received discovery request from {receivedResult.RemoteEndPoint.Address}:{receivedResult.RemoteEndPoint.Port}: {requestText}");

                if (requestText == ExpectedRequest)
                {
                    byte[] responseData = Encoding.UTF8.GetBytes(ResponseMessage);
                    await udpClient.SendAsync(responseData, responseData.Length, receivedResult.RemoteEndPoint);
                    SimpleLogger.LogInformation($"Sent discovery response to {receivedResult.RemoteEndPoint.Address}:{receivedResult.RemoteEndPoint.Port}: {ResponseMessage}");
                }
            }
            catch (OperationCanceledException)
            {
                // Ignore cancellation exceptions
                SimpleLogger.LogWarning("Discovery listener is stopping due to cancellation.");
            }
            catch (Exception e)
            {
                SimpleLogger.LogWarning($"Error while listening for discovery requests: {e.Message}");
            }
        }
    }

    public override void Dispose()
    {
        udpClient?.Dispose();
        base.Dispose();
    }
}