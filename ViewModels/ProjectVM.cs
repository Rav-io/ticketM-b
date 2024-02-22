using System.ComponentModel.DataAnnotations;

namespace projekt_zespolowy.ViewModels
{
    public class ProjectVM
    {
        [Required]
        public string ProjectName { get; set; }
    }
}
