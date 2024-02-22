using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

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

            var roles = new List<Role>
            {
                new Role { Id = 1,Name = "Admin" },
                new Role { Id = 2,Name = "User" }
            };
            modelBuilder.Entity<Role>().HasData(roles);

            var adminUser = new User { Id = 1,UserName = "admin", Password = "adminpass", RoleId = roles[0].Id };
            modelBuilder.Entity<User>().HasData(adminUser);

            var projects = new List<Project>
            {
                new Project { Id = 1, ProjectName = "Project A" },
                new Project { Id = 2, ProjectName = "Project B" }
            };
            modelBuilder.Entity<Project>().HasData(projects);

            var tasks = new List<Task>
            {
                new Task { Id = 1,TaskName = "Task 1", TaskDescription = "Description 1", TaskStatus = 0, ProjectId = projects[0].Id },
                new Task { Id = 2,TaskName = "Task 2", TaskDescription = "Description 2", TaskStatus = 1, ProjectId = projects[1].Id }
            };
            modelBuilder.Entity<Task>().HasData(tasks);

            modelBuilder.Entity<UserTask>().HasData(new { UserId = adminUser.Id, TaskId = tasks[0].Id });
        }
    }
}
