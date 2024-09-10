using Microsoft.AspNetCore.SignalR;

namespace SignalR.Chart.API.Hubs;

public class CovidHub : Hub
{
    public async Task GetCovidList()
    {
        await Clients.All.SendAsync("ReceiveCovidList", "Получи данные Covid-19 из базы");
    }
}