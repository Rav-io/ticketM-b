using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using projekt_zespolowy.Models;
using System;

namespace projekt_zespolowy.DTO
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public int TaskStatus { get; set; }
        public List<UserDto> Users { get; set; }
        public int ProjectId { get; set; }
    }
}