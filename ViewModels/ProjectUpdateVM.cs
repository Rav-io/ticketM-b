using System.ComponentModel.DataAnnotations;

namespace projekt_zespolowy.ViewModels
{
    public class ProjectUpdateVM
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
    }
}
