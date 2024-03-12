using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels
{
    public class EditTaskVM
    {
        [Required]
        public string TaskName { get; set; }
        [Required]
        public string TaskDescription { get; set; }
        [Required]
        public int TaskStatus { get; set; }
        public List<int>? UserIds { get; set; }
    }
}
