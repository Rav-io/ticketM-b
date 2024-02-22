using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace ticketmanager.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}