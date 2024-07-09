using Microsoft.AspNetCore.SignalR;
using SignalR.Web.Models;

namespace SignalR.Web.Hubs;

public class ExampleTypeSafeHub : Hub<IExampleTypeSafeHub>
{
    private static int _connectedClientCount = 0;

    public async Task BroadcastMessageToAllClient(string message)
    {
        await Clients.All.ReceiveMessageForAllClient(message);
    }

    public async Task BroadcastTypedMessageToAllClient(Product product)
    {
        await Clients.All.ReceiveTypesMessageForAllClient(product);
    }

    public async Task BroadcastMessageToCallerClient(string message)
    {
        await Clients.Caller.ReceiveMessageForCallerClient(message);
    }

    public async Task BroadcastMessageToOthersClient(string message)
    {
        await Clients.Others.ReceiveMessageOthersCallerClient(message);
    }

    public async Task BroadcastMessageToIndividualClient(string connectionId, string message)
    {
        await Clients.Client(connectionId).ReceiveMessageForIndividualClient(message);
    }

    public async Task BroadcastMessageToGroupClient(string groupName, string message)
    {
        await Clients.Group(groupName).ReceiveMessageForGroupClient(message);
    }

    //Group methods
    public async Task AddGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.ReceiveMessageForCallerClient($"You were added to group {groupName}");
        await Clients.Group(groupName)
            .ReceiveMessageForGroupClient($"This user {Context.ConnectionId} added to group {groupName}");
    }

    public async Task RemoveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.ReceiveMessageForCallerClient($"You were removed from group {groupName}");
        await Clients.Group(groupName)
            .ReceiveMessageForGroupClient($"This user {Context.ConnectionId} removed from group {groupName}");
    }

    //Stream
    public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunks)
    {
        await foreach (var name in nameAsChunks)
        {
            await Task.Delay(1000);
            await Clients.All.ReceiveMessageAsStreamForAllClient(name);
        }
    }

    public async Task BroadcastStreamProductToAllClient(IAsyncEnumerable<Product> productAsChunks)
    {
        await foreach (var product in productAsChunks)
        {
            await Task.Delay(1000);
            await Clients.All.ReceiveProductAsStreamForAllClient(product);
        }
    }

    public async IAsyncEnumerable<string> BroadcastFromHubToClients(int count)
    {
        foreach (var item in Enumerable.Range(1, count).ToList())
        {
            await Task.Delay(1000);
            yield return $"{item}. data";
        }
    }

    //Settings
    public override async Task OnConnectedAsync()
    {
        _connectedClientCount++;
        await Clients.All.ReceiveConnectedClientCountAllClient(_connectedClientCount);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedClientCount--;
        await Clients.All.ReceiveConnectedClientCountAllClient(_connectedClientCount);
        await base.OnDisconnectedAsync(exception);
    }
}