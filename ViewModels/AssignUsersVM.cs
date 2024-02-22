using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels
{
    public class AssignUsersVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public List<int> UserId { get; set; }
    }
}
