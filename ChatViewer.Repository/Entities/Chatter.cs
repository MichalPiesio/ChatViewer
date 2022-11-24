using System.ComponentModel.DataAnnotations;

namespace ChatViewer.Repository.Entities;

public class Chatter
{
    [Key]
    public Guid ChatterId { get; set; }
    
    public string Name { get; set; }
    
    public virtual List<ChatEvent> ChatEvents { get; set; }
    public virtual List<ChatEvent> ChatEvents2 { get; set; }
}