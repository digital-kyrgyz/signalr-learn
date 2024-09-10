using SignalR.Chart.API.Enums;

namespace SignalR.Chart.API.Models;

public class Covid
{
    public int Id { get; set; }
    public ECity City { get; set; }
    public int Count { get; set; }
    public DateTime CovidDate { get; set; }
}