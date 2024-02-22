using System.ComponentModel.DataAnnotations;

namespace projekt_zespolowy.ViewModels
{
    public class AssignUsersVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public List<int> UserId { get; set; }
    }
}
