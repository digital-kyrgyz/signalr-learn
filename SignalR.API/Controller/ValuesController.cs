using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.API.Hubs;

namespace SignalR.API.Controller;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly IHubContext<MyHub> _myHub;
    public ValuesController(IHubContext<MyHub> myHub)
    {
        _myHub = myHub;
    }
    [HttpGet]
    public async Task<IActionResult> Get(string message)
    {
        await _myHub.Clients.All.SendAsync("ReceiveMessageForAllClient", message);
        return Ok();
    }
}