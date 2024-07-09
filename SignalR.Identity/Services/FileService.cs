using System.Threading.Channels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalR.Identity.Infrastructure;
using SignalR.Identity.Models;

namespace SignalR.Identity.Services;

public class FileService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly Channel<(string, List<Product>)> _channel;
    
    public FileService(
        AppDbContext context,
        IHttpContextAccessor httpContextAccessor,
        UserManager<IdentityUser> userManager, Channel<(string, List<Product>)> channel)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _channel = channel;
    }

    public async Task<bool> AddMessageToQueue()
    {
        var userId = _userManager.GetUserId(_httpContextAccessor!.HttpContext!.User);
        var products = await _context.Products.Where(x => x.UserId == userId).ToListAsync();
        return _channel.Writer.TryWrite((userId, products));
    }

}