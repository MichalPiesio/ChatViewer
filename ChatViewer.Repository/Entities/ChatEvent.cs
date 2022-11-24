using System.ComponentModel.DataAnnotations;

namespace ChatViewer.Repository.Entities;

public class ChatEvent
{
    [Key]
    public Guid ChatEventId { get; set; }
    
    public string ChatEventName { get; set; }
    public Guid ChatterId { get; set; }
    public Guid? Chatter2Id { get; set; }
    public DateTime EventDateTime { get; set; }
    public string Text { get; set; }
    
    public virtual ChatEventType ChatEventType { get; set; }
    public virtual Chatter Chatter { get; set; }
    public virtual Chatter Chatter2{ get; set; }
}