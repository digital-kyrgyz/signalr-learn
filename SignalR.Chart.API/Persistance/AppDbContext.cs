using Microsoft.EntityFrameworkCore;
using SignalR.Chart.API.Models;

namespace SignalR.Chart.API.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Covid> Covids { get; set; }
}