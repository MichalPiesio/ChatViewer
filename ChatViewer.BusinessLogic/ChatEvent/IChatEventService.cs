using ChatViewer.Contracts.Dto;

namespace ChatViewer.BusinessLogic.ChatEvent;

public interface IChatEventService
{
    Task<List<ChatEventDto>> GetChatEventsByMinute();
    Task<List<ChatEventAggregateDto>> GetChatEventHourlyAggregate();
}