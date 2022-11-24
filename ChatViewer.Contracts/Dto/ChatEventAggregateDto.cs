namespace ChatViewer.Contracts.Dto;

public class ChatEventAggregateDto
{
    public DateTime DateTime { get; set; }
    public List<ChatEventAggregateDetailDto> Details { get; set; } = new();
}

public class ChatEventAggregateDetailDto
{
    public string ChatEventType { get; set; }
    public int Count1 { get; set; }
    public int Count2 { get; set; }
}
