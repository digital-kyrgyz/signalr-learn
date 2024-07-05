using Microsoft.AspNetCore.SignalR.Client;
using SignalR.ConsoleApp.Models;

Console.WriteLine("SignalR Console Client");

var connection = new HubConnectionBuilder().WithUrl("https://localhost:44342/exampleTypeSafeHub").Build();

connection.StartAsync().ContinueWith((result) =>
{
    Console.WriteLine(result.IsCompletedSuccessfully ? "Connected" : "Connected failed");
});

//Subscribe
connection.On<Product>("ReceiveTypesMessageForAllClient",
    (product) => { Console.WriteLine($"Received Message: {product.Id} - {product.Name} - {product.Price}"); });

while (true)
{
    var key = Console.ReadLine();
    if (key == "exit") break;
    var product = new Product(200, "pencil console", 250);
    await connection.InvokeAsync("BroadcastTypedMessageToAllClient", product);
}