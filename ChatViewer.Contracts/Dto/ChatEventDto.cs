namespace ChatViewer.Contracts.Dto;

public class ChatEventDto
{
    public string Chatter { get; set; }
    public string Chatter2 { get; set; }
    public string Text { get; set; }
    public string ChatEventType { get; set; }
    public DateTime EventDateTime { get; set; }
}