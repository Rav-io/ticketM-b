using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels;

public class UserLoginVM
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}