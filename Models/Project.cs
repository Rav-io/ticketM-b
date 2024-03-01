using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ticketmanager.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public List<User> Users { get; set; }
        public List<Task> Tasks { get; set; }
        public IList<UserProject> UserProjects { get; set; }
    }
}
