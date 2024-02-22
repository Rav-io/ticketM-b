using System.ComponentModel.DataAnnotations;

namespace projekt_zespolowy.ViewModels;

public class UserViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
    [Required]
    public string Role { get; set; }
}