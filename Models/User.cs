using System.ComponentModel.DataAnnotations;

namespace projekt_zespolowy.Models;

public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public IList<Task> Tasks { get; set; }
    }
