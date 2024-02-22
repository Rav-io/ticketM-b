using System.ComponentModel.DataAnnotations.Schema;

namespace ticketmanager.Models;

public class UserTask
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int TaskId { get; set; }
    public Task Task { get; set; }
}