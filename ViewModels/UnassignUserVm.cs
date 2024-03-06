using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels
{
    public class UnassignUserVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
