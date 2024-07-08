using System.ComponentModel.DataAnnotations;

namespace SignalR.Identity.Models;

public record SignUpDto([Required]string Email, [Required]string Password, [Required]string ConfirmPassword);