using System.ComponentModel.DataAnnotations;

namespace ChatViewer.Repository.Entities;

public class ChatEventType
{
    [Key]
    public string ChatEventName { get; set; } 
    
    public virtual List<ChatEvent> ChatEvents { get; set; }
}