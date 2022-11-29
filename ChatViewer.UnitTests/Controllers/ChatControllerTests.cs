using ChatViewer.BusinessLogic.ChatEvent;
using ChatViewer.Contracts;
using ChatViewer.Contracts.Dto;
using ChatViewer.Controllers;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace ChatViewer.UnitTests.Controllers;

public class ChatControllerTests
{
    private readonly IChatEventService _chatEventService;
    private readonly ChatController _chatController;

    public ChatControllerTests()
    {
        _chatEventService = A.Fake<IChatEventService>();
        _chatController = new ChatController(_chatEventService);
    }

    [Fact]
    public async Task GetByMinute_ReturnsDto()
    {
        // ARRANGE
        var expectedChatEventDtos = new List<ChatEventDto>()
        {
            new()
            {
                ChatEventType = ChatEventTypes.Comment,
                Chatter = "Someone",
                EventDateTime = new DateTime(2022, 5, 7),
                Text = "Something"
            }
        };

        A.CallTo(() => _chatEventService.GetChatEventsByMinute()).Returns(expectedChatEventDtos);
        
        // ACT
        var actualChatEventDtos = await _chatController.GetByMinute();
        
        // ASSERT 
        actualChatEventDtos.Should().BeEquivalentTo(expectedChatEventDtos);
    }
    
    [Fact]
    public async Task GetByHour_ReturnsDto()
    {
        // ARRANGE
        var expectedChatEventAggregateDtos = new List<ChatEventAggregateDto>()
        {
            new()
            {
                DateTime = new DateTime(2022, 4, 6, 12, 0, 0),
                Details = new List<ChatEventAggregateDetailDto>()
                {
                    new()
                    {
                        ChatEventType = ChatEventTypes.Comment,
                        Count1 = 10,
                    }
                }
            }
        };

        A.CallTo(() => _chatEventService.GetChatEventHourlyAggregate()).Returns(expectedChatEventAggregateDtos);
        
        // ACT
        var actualChatEventDtos = await _chatController.GetByHour();
        
        // ASSERT 
        actualChatEventDtos.Should().BeEquivalentTo(expectedChatEventAggregateDtos);
    }
    
    [Fact]
    public async Task GetByDay_ReturnsDto()
    {
        // ARRANGE
        var expectedChatEventAggregateDtos = new List<ChatEventAggregateDto>()
        {
            new()
            {
                DateTime = new DateTime(2022, 4, 6, 0, 0, 0),
                Details = new List<ChatEventAggregateDetailDto>()
                {
                    new()
                    {
                        ChatEventType = ChatEventTypes.Comment,
                        Count1 = 10,
                    }
                }
            }
        };

        A.CallTo(() => _chatEventService.GetChatEventDailyAggregate()).Returns(expectedChatEventAggregateDtos);
        
        // ACT
        var actualChatEventDtos = await _chatController.GetByDay();
        
        // ASSERT 
        actualChatEventDtos.Should().BeEquivalentTo(expectedChatEventAggregateDtos);
    }
}