using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace projekt_zespolowy.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}