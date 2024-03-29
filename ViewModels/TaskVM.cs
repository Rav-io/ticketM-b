using System.ComponentModel.DataAnnotations;

namespace ticketmanager.ViewModels
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
        [Required]
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set;}
    }
}
