using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels
{
    public class ProjectDeleteVM
    {
        [Required]
        public int Id { get; set; }
    }
}
