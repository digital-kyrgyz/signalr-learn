using System.ComponentModel.DataAnnotations;

namespace SignalR.Identity.Models;

public record SignInDto([Required]string Email, [Required]string Password);