using System.ComponentModel.DataAnnotations;

namespace ticketmanager.Models {
    public class User
    {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public int RoleId { get; set; }
            public Role Role { get; set; }
            public Project? Project { get; set; }
            public IList<UserTask> UserTasks { get; set; }
            public ICollection<Task> Tasks { get; set; }
            public IList<UserProject> UserProjects { get; set; }
     }
}
