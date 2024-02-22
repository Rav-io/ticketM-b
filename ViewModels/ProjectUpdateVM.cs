using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels
{
    public class ProjectUpdateVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
    }
}
