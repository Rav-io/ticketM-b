using System.ComponentModel.DataAnnotations;

namespace projekt_zespolowy.ViewModels
{
    public class TaskVM
    {
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string TaskDescription { get; set; }
        [Required]
        public int TaskStatus { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public List<int>? UserIds { get; set; }
    }
}
