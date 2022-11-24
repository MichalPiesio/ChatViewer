using ChatViewer.Contracts;
using ChatViewer.Contracts.Dto;
using ChatViewer.Repository;
using Microsoft.EntityFrameworkCore;

namespace ChatViewer.BusinessLogic.ChatEvent;

public class ChatEventService : IChatEventService
{
    private readonly ChatViewerDbContext _dbContext;

    public ChatEventService(ChatViewerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ChatEventDto>> GetChatEventsByMinute()
    {
        return (await _dbContext.ChatEvent
                .Include(x => x.Chatter)
                .Include(x => x.Chatter2)
                .OrderBy(x => x.EventDateTime)
                .ToListAsync())
            .Select(Map)
            .ToList();
    }

    public async Task<List<ChatEventAggregateDto>> GetChatEventHourlyAggregate()
    {
        var aggregates = new List<ChatEventAggregateDto>();
        var groups = await _dbContext.ChatEvent
            .GroupBy(x => new { x.EventDateTime.Date, x.EventDateTime.Hour, x.ChatEventName })
            .ToListAsync();

        foreach (var group in groups)
        {
            var date = group.Key.Date;
            var hour = group.Key.Hour;
            var chatEventName = group.Key.ChatEventName;
            
            var aggregateDetail = AddDtosToAggregates(aggregates, date, hour, chatEventName);
            UpdateAggregates(chatEventName, aggregateDetail, group.ToList());
        }

        return aggregates;
    }

    private static ChatEventAggregateDetailDto AddDtosToAggregates(List<ChatEventAggregateDto> aggregates, DateTime date, int hour, string chatEventName)
    {
        var aggregate = aggregates.FirstOrDefault(x =>
            x.DateTime == date.AddHours(hour));

        if (aggregate == null)
        {
            aggregate = new ChatEventAggregateDto
            {
                DateTime = date.AddHours(hour),
            };
            aggregates.Add(aggregate);
        }

        var aggregateDetail = new ChatEventAggregateDetailDto
        {
            ChatEventType = chatEventName
        };
        aggregate.Details.Add(aggregateDetail);
        return aggregateDetail;
    }

    private static void UpdateAggregates(string chatEventName, ChatEventAggregateDetailDto aggregateDetail, List<Repository.Entities.ChatEvent> chatEvents)
    {
        switch (chatEventName)
        {
            case ChatEventTypes.Comment:
                aggregateDetail.Count1 = chatEvents.Count;
                break;

            case ChatEventTypes.EnterTheRoom:
            case ChatEventTypes.LeaveTheRoom:
                aggregateDetail.Count1 = chatEvents
                    .Select(x => x.ChatterId)
                    .Distinct()
                    .Count();
                break;

            case ChatEventTypes.HighFive:
                aggregateDetail.Count1 = chatEvents
                    .Select(x => x.ChatterId)
                    .Distinct()
                    .Count();
                aggregateDetail.Count2 = chatEvents
                    .Select(x => x.Chatter2Id)
                    .Distinct()
                    .Count();
                break;
        }
    }

    private static ChatEventDto Map(Repository.Entities.ChatEvent arg)
    {
        return new ChatEventDto
        {
            ChatEventType = arg.ChatEventName,
            Chatter = arg.Chatter.Name,
            Chatter2 = arg.Chatter2?.Name,
            EventDateTime = arg.EventDateTime,
            Text = arg.Text
        };
    }
}