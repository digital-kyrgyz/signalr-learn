using Microsoft.AspNetCore.SignalR.Client;
using SignalR.WorkerService.Models;

namespace SignalR.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private HubConnection? _connection;
    private readonly IConfiguration _configuration;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _connection = new HubConnectionBuilder().WithUrl(_configuration.GetSection("SignalR")["Hub"]!).Build();
        
        _connection?.StartAsync().ContinueWith((result) =>
        {
            _logger.LogInformation(result.IsCompletedSuccessfully ? "Connected" : "Connection failed");
        });
        
        return base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _connection!.StopAsync(cancellationToken);
        await _connection!.DisposeAsync();
        
        await base.StopAsync(cancellationToken);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _connection!.On<Product>("ReceiveTypesMessageForAllClient",
            (product) => { _logger.LogInformation($"Received Message: {product.Id} - {product.Name} - {product.Price}"); });
        return Task.CompletedTask;
    }
}