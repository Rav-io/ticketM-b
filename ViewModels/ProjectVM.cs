using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels
{
    public class ProjectVM
    {
        [Required]
        public string ProjectName { get; set; }
    }
}
