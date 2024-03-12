using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using BCrypt.Net;

namespace ticketmanager.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<Task>()
                .HasMany(t => t.Users)
                .WithMany(u => u.Tasks)
                .UsingEntity<UserTask>(
                    j => j.ToTable("UserTasks")
                        .HasKey(ut => new { ut.UserId, ut.TaskId })
                );

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.User)
                .WithMany(u => u.UserTasks)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserTask>()
                .HasOne(ut => ut.Task)
                .WithMany(t => t.UserTasks)
                .HasForeignKey(ut => ut.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);

            var roles = new List<Role>
            {
                new Role { Id = 1,Name = "Admin" },
                new Role { Id = 2,Name = "User" }
            };
            modelBuilder.Entity<Role>().HasData(roles);

            var projects = new List<Project>
            {
                new Project { Id = 1, ProjectName = "Project A" },
                new Project { Id = 2, ProjectName = "Project B" },
                new Project { Id = 3, ProjectName = "Project C" },
                new Project { Id = 4, ProjectName = "Project D" },
            };
            modelBuilder.Entity<Project>().HasData(projects);

            var users = new List<User>
            {
                new User { Id = 1, UserName = "admin", Password = BCrypt.Net.BCrypt.HashPassword("admin"), RoleId = roles[0].Id},
                new User { Id = 2, UserName = "admin2", Password = BCrypt.Net.BCrypt.HashPassword("admin2"), RoleId = roles[0].Id},
                new User { Id = 3, UserName = "user", Password = BCrypt.Net.BCrypt.HashPassword("user"), RoleId = roles[1].Id},
                new User { Id = 4, UserName = "user2", Password = BCrypt.Net.BCrypt.HashPassword("user2"), RoleId = roles[1].Id},
                new User { Id = 5, UserName = "user3", Password = BCrypt.Net.BCrypt.HashPassword("user3"), RoleId = roles[1].Id},
                new User { Id = 6, UserName = "user4", Password = BCrypt.Net.BCrypt.HashPassword("user4"), RoleId = roles[1].Id},
                new User { Id = 7, UserName = "user5", Password = BCrypt.Net.BCrypt.HashPassword("user5"), RoleId = roles[1].Id},
                new User { Id = 8, UserName = "user6", Password = BCrypt.Net.BCrypt.HashPassword("user6"), RoleId = roles[1].Id},
                new User { Id = 9, UserName = "user7", Password = BCrypt.Net.BCrypt.HashPassword("user7"), RoleId = roles[1].Id},
            };
            modelBuilder.Entity<User>().HasData(users);

            var tasks = new List<Task>
            {
                new Task { Id = 1, TaskName = "Task 1", TaskDescription = "Description 1", TaskStatus = 0, ProjectId = projects[0].Id, CreationDate = DateTime.Now, CreatedBy = "admin" },
                new Task { Id = 2, TaskName = "Task 2", TaskDescription = "Description 2", TaskStatus = 1, ProjectId = projects[1].Id, CreationDate = DateTime.Now, CreatedBy = "admin" },
                new Task { Id = 3, TaskName = "Task 3", TaskDescription = "Description 3", TaskStatus = 0, ProjectId = projects[2].Id, CreationDate = DateTime.Now, CreatedBy = "admin" },
                new Task { Id = 4, TaskName = "Task 4", TaskDescription = "Description 4", TaskStatus = 1, ProjectId = projects[3].Id, CreationDate = DateTime.Now, CreatedBy = "admin" },
            };
            modelBuilder.Entity<Task>().HasData(tasks);

            var userTasks = new List<UserTask>
            {
                new UserTask { UserId = users[3].Id, TaskId = tasks[1].Id },
                new UserTask { UserId = users[3].Id, TaskId = tasks[2].Id },
                new UserTask { UserId = users[3].Id, TaskId = tasks[3].Id },
                new UserTask { UserId = users[4].Id, TaskId = tasks[2].Id },
                new UserTask { UserId = users[5].Id, TaskId = tasks[3].Id },
                new UserTask { UserId = users[6].Id, TaskId = tasks[2].Id },
            };

            modelBuilder.Entity<UserTask>().HasData(userTasks);

            var userProjects = new List<UserProject>
            {
                new UserProject { UserId = users[0].Id, ProjectId = projects[1].Id },
                new UserProject { UserId = users[0].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[0].Id, ProjectId = projects[3].Id },
                new UserProject { UserId = users[0].Id, ProjectId = projects[0].Id },
                new UserProject { UserId = users[1].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[2].Id, ProjectId = projects[3].Id },
                new UserProject { UserId = users[3].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[4].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[5].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[6].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[7].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[8].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[2].Id, ProjectId = projects[2].Id },
                new UserProject { UserId = users[3].Id, ProjectId = projects[3].Id },
                new UserProject { UserId = users[4].Id, ProjectId = projects[3].Id },
                new UserProject { UserId = users[5].Id, ProjectId = projects[1].Id },
            };

            modelBuilder.Entity<UserProject>().HasData(userProjects);
        }
    }
}
