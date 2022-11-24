using ChatViewer.BusinessLogic.ChatEvent;
using ChatViewer.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ChatViewer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatEventService _chatEventService;
        public ChatController(IChatEventService chatEventService)
        {
            _chatEventService = chatEventService;
        }

        [HttpGet]
        [Route("minute")]
        public async Task<List<ChatEventDto>> GetByMinute()
        {
            return await _chatEventService.GetChatEventsByMinute();
        }
        
        [HttpGet]
        [Route("hour")]
        public async Task<List<ChatEventAggregateDto>> GetByHour()
        {
            return await _chatEventService.GetChatEventHourlyAggregate();
        }

       
    }
}