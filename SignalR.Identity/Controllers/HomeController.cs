using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignalR.Identity.Infrastructure;
using SignalR.Identity.Models;
using SignalR.Identity.Services;

namespace SignalR.Identity.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly AppDbContext _appDbContext;
    private readonly FileService _fileService;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, AppDbContext appDbContext, FileService fileService)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _appDbContext = appDbContext;
        _fileService = fileService;
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

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var user = new IdentityUser()
        {
            UserName = dto.Email,
            Email = dto.Email,
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return RedirectToAction(nameof(SignIn));
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(SignInDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var hasUser = await _userManager.FindByEmailAsync(dto.Email);

        if (hasUser is null)
        {
            ModelState.AddModelError(string.Empty, "Email or password is wrong");
        }

        var result = await _signInManager.PasswordSignInAsync(hasUser, dto.Password, true, true);

        if (!result.Succeeded)
            ModelState.AddModelError(string.Empty, "Email or Password is wrong");

        return RedirectToAction(nameof(Index));
    }

    public IActionResult SignIn()
    {
        return View();
    }

    public async Task<IActionResult> ProductList()
    {
        var user = await _userManager.FindByEmailAsync("melis.archabaev.kg@gmail.com");

        if (_appDbContext.Products.Any(x => x.UserId == user!.Id))
        {
            var products = _appDbContext.Products.Where(x => x.UserId == user.Id).ToList();
            return View(products);
        }

        var productList = new List<Product>()
        {
            new Product() { Name = "Pen 1", Description = "Description 1", Price = 100, UserId = user!.Id },
            new Product() { Name = "Pen 2", Description = "Description 2", Price = 200, UserId = user!.Id },
            new Product() { Name = "Pen 3", Description = "Description 3", Price = 300, UserId = user!.Id },
            new Product() { Name = "Pen 4", Description = "Description 4", Price = 400, UserId = user!.Id },
            new Product() { Name = "Pen 5", Description = "Description 5", Price = 500, UserId = user!.Id },
        };
        await _appDbContext.Products.AddRangeAsync(productList);
        await _appDbContext.SaveChangesAsync();

        return View(productList);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> CreateExcel()
    {
        var response = new
        {
            Status = await _fileService.AddMessageToQueue(),
        };
        return Json(response);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}