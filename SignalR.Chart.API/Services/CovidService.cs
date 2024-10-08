using Microsoft.AspNetCore.SignalR;
using SignalR.Chart.API.Hubs;
using SignalR.Chart.API.Models;
using SignalR.Chart.API.Persistance;

namespace SignalR.Chart.API.Services;

public class CovidService
{
    private readonly AppDbContext _context;
    private readonly IHubContext<CovidHub> _hubContext;

    public CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public IQueryable<Covid> GetList()
    {
        return _context.Covids.AsQueryable();
    }

    public async Task SaveCOvid(Covid covid)
    {
        await _context.Covids.AddAsync(covid);
        await _context.SaveChangesAsync();
        await _hubContext.Clients.All.SendAsync("ReceiveCovidList", "Data");
    }
}