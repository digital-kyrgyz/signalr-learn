using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Hubs;

public class ExampleHub : Hub
{
    public async Task BroadcastMessageToAllClient(string message)
    {
        await Clients.All.SendAsync("ReceiveMessageForAllClient", message);
    }
}