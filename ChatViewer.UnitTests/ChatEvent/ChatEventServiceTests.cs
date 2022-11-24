using ChatViewer.BusinessLogic.ChatEvent;
using ChatViewer.Contracts;
using ChatViewer.Contracts.Dto;
using ChatViewer.Repository;
using ChatViewer.Repository.Entities;
using FluentAssertions;
using Xunit;

namespace ChatViewer.UnitTests.ChatEvent;

public class ChatEventServiceTests : IDisposable
{
    private readonly ChatViewerDbContext _dbContext;
    private readonly IChatEventService _chatEventService;
    private readonly ChatViewerDbContextFactory _factory;

    public ChatEventServiceTests()
    {
        _factory = new ChatViewerDbContextFactory();
        _dbContext = _factory.CreateContext();
        _chatEventService = new ChatEventService(_dbContext);
    }

    [Theory]
    [InlineData(ChatEventTypes.Comment, "I'm commenting something", null)]
    [InlineData(ChatEventTypes.HighFive, null, "6f2175b2-7e44-4a74-b514-f2c8f5e37026")]
    [InlineData(ChatEventTypes.EnterTheRoom, null, null)]
    [InlineData(ChatEventTypes.LeaveTheRoom, null, null)]
    public async Task GetChatEventsByMinute_ReturnsCorrectDtoForChatEventType(string type, string text, string chatter2Id)
    {
        // ARRANGE
        await SetUpData(type, text, chatter2Id);

        var expectedDto = new ChatEventDto
        {
            ChatEventType = type,
            Chatter = "Chris",
            Chatter2 = !string.IsNullOrEmpty(chatter2Id) ? "John" : null,
            Text = text,
            EventDateTime = new DateTime(2022, 5, 6, 13, 0, 1)
        };
        
        // ACT
        var actualDto = await _chatEventService.GetChatEventsByMinute();
        
        // ASSERT
        actualDto.Single().Should().BeEquivalentTo(expectedDto);
    }
    
    [Theory]
    [InlineData(ChatEventTypes.Comment, "I'm commenting something", null, 1, 0)]
    [InlineData(ChatEventTypes.HighFive, null, "6f2175b2-7e44-4a74-b514-f2c8f5e37026", 1, 1)]
    [InlineData(ChatEventTypes.EnterTheRoom, null, null, 1, 0)]
    [InlineData(ChatEventTypes.LeaveTheRoom, null, null, 1, 0)]
    public async Task GetChatEventHourlyAggregate_ReturnsCorrectDtoForChatEventType(string type, string text, string chatter2Id, int count1, int count2)
    {
        // ARRANGE
        await SetUpData(type, text, chatter2Id);

        var expectedDto = new ChatEventAggregateDto
        {
            DateTime = new DateTime(2022, 5, 6, 13, 0, 0),
            Details = new()
            {
                new ChatEventAggregateDetailDto
                {
                    ChatEventType = type,
                    Count1 = count1,
                    Count2 = count2
                }
            }
        };
        
        // ACT
        var actualDto = await _chatEventService.GetChatEventHourlyAggregate();
        
        // ASSERT
        actualDto.Single().Should().BeEquivalentTo(expectedDto);
    }
    
    [Theory]
    [InlineData(ChatEventTypes.Comment, "I'm commenting something", null, 5, 0)] // should count each one
    [InlineData(ChatEventTypes.HighFive, null, "6f2175b2-7e44-4a74-b514-f2c8f5e37026", 1, 1)] // should count only distinct users
    [InlineData(ChatEventTypes.EnterTheRoom, null, null, 1, 0)] // should count only distinct users
    [InlineData(ChatEventTypes.LeaveTheRoom, null, null, 1, 0)] // should count only distinct users
    public async Task GetChatEventHourlyAggregate_ReturnsCorrectCountOfDuplicateEvents(string type, string text, string chatter2Id, int count1, int count2)
    {
        // ARRANGE
        await SetUpData(type, text, chatter2Id, 5);

        var expectedDto = new ChatEventAggregateDto
        {
            DateTime = new DateTime(2022, 5, 6, 13, 0, 0),
            Details = new()
            {
                new ChatEventAggregateDetailDto
                {
                    ChatEventType = type,
                    Count1 = count1,
                    Count2 = count2
                }
            }
        };
        
        // ACT
        var actualDto = await _chatEventService.GetChatEventHourlyAggregate();
        
        // ASSERT
        actualDto.Single().Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public async Task GetChatEventHourlyAggregate_ReturnsAggregatesPerEachHour()
    {
        // ARRANGE
        var chatter1 = new Chatter
        {
            ChatterId = Guid.NewGuid(),
            Name = "Chris"
        };
        _dbContext.Add(chatter1);
        
        var chatEvent1 = new Repository.Entities.ChatEvent
        {
            ChatEventId = Guid.NewGuid(),
            ChatEventName = ChatEventTypes.EnterTheRoom,
            ChatterId = chatter1.ChatterId,
            EventDateTime = new DateTime(2022, 5, 6, 13, 0, 1)
        };
        _dbContext.Add(chatEvent1);
        
        var chatEvent2 = new Repository.Entities.ChatEvent
        {
            ChatEventId = Guid.NewGuid(),
            ChatEventName = ChatEventTypes.EnterTheRoom,
            ChatterId = chatter1.ChatterId,
            EventDateTime = new DateTime(2022, 5, 6, 14, 0, 1)
        };
        _dbContext.Add(chatEvent2);
        
        await _dbContext.SaveChangesAsync();

        var expectedDtos = new List<ChatEventAggregateDto>
        {
            new()
            {
                DateTime = new DateTime(2022, 5, 6, 13, 0, 0),
                Details = new()
                {
                    new ChatEventAggregateDetailDto
                    {
                        ChatEventType = ChatEventTypes.EnterTheRoom,
                        Count1 = 1,
                    }
                }
            },
            new()
            {
                DateTime = new DateTime(2022, 5, 6, 14, 0, 0),
                Details = new()
                {
                    new ChatEventAggregateDetailDto
                    {
                        ChatEventType = ChatEventTypes.EnterTheRoom,
                        Count1 = 1,
                    }
                }
            }
        };
        
        // ACT
        var actualDtos = await _chatEventService.GetChatEventHourlyAggregate();
        
        // ASSERT
        actualDtos.Should().BeEquivalentTo(expectedDtos);
    }

    private async Task SetUpData(string type, string text, string chatter2Id, int eventsCount = 1)
    {
        var chatter1 = new Chatter
        {
            ChatterId = Guid.NewGuid(),
            Name = "Chris"
        };
        _dbContext.Add(chatter1);

        if (!string.IsNullOrEmpty(chatter2Id))
        {
            var chatter2 = new Chatter
            {
                ChatterId = Guid.Parse(chatter2Id),
                Name = "John"
            };
            _dbContext.Add(chatter2);
        }

        for (int i = 0; i < eventsCount; i++)
        {
            var chatEvent = new Repository.Entities.ChatEvent
            {
                ChatEventId = Guid.NewGuid(),
                ChatEventName = type,
                ChatterId = chatter1.ChatterId,
                Chatter2Id = !string.IsNullOrEmpty(chatter2Id) ? Guid.Parse(chatter2Id) : null,
                Text = text,
                EventDateTime = new DateTime(2022, 5, 6, 13, 0, 1)
            };
            _dbContext.Add(chatEvent);
        }

        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _factory.Dispose();
    }
}