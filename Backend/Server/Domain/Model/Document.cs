using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models;

namespace Domain.Model;

public class Document:ITimeStampedModel
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }
    
    [Column(TypeName = "bytea")]
    public byte[] DocumentEntered { get; set; }
    
    [Column(TypeName = "bytea")]
    public byte[] DocumentReady { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }

    public Document(long userId, byte[] documentEntered, byte[] documentReady)
    {
        UserId = userId;
        DocumentEntered = documentEntered;
        DocumentReady = documentReady;
    }
}