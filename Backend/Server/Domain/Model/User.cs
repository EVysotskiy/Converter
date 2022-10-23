using System.ComponentModel.DataAnnotations;
using Domain.Models;

namespace Domain.Model;

public class User : ITimeStampedModel
{
    [Key]
    public long IdChat { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public User(long idChat)
    {
        IdChat = idChat;
    }
}