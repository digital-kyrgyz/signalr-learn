using SignalR.Web.Models;

namespace SignalR.Web.Hubs;

public interface IExampleTypeSafeHub
{
    Task ReceiveMessageForAllClient(string message);
    Task ReceiveTypesMessageForAllClient(Product product);
    Task ReceiveConnectedClientCountAllClient(int clientCount);
    Task ReceiveMessageForCallerClient(string message);
    Task ReceiveMessageOthersCallerClient(string message);
    Task ReceiveMessageForIndividualClient(string message);
    Task ReceiveMessageForGroupClient(string message);
    Task ReceiveMessageAsStreamForAllClient(string name);
    Task ReceiveProductAsStreamForAllClient(Product product);
}