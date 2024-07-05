using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR.Identity.Models;

namespace SignalR.Identity.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    
    public IActionResult SignUp(SignUpDto dto)
    {
        if (!ModelState.IsValid) return View();
        var user = new IdentityUser()
        {
            UserName = dto.Email,
            Email = dto.Email,
            
        }
    }
    
    public IActionResult SignIn()
    {
        return View();
    }
    
    public IActionResult ProductList()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}