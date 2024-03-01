using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticketmanager.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        [MaxLength(1000)]
        public string TaskDescription { get; set; }
        public int TaskStatus { get; set; }
        public ICollection<User> Users { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }
    }
}
