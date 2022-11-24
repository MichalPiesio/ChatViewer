using ChatViewer.Contracts;
using ChatViewer.Repository;
using ChatViewer.Repository.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChatViewer.Controllers
{
    [ApiController]
    [Route("dataseed")]
    public class DataSeedController : ControllerBase
    {
        private ChatViewerDbContext _dbContext;

        public DataSeedController(ChatViewerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [Route("seed")]
        public void Seed()
        {
            var rng = new Random();
            
            _dbContext.Chatter.AddRange(Enumerable.Range(1, 5).Select(_ => new Chatter
            {
                ChatterId = Guid.NewGuid(),
                Name = _firstNames.ElementAt(rng.Next(0, _firstNames.Count))
            }).ToList());
            
            _dbContext.SaveChanges();

            var chatters = _dbContext.Chatter.ToList();
            var chatEventTypes = _dbContext.ChatEventType.ToList();
            
            _dbContext.ChatEvent.AddRange(Enumerable.Range(1, 20).Select(x =>
            {
                var chatter = chatters.ElementAt(rng.Next(chatters.Count/2, chatters.Count));
                var chatter2 = chatters.ElementAt(rng.Next(0, chatters.Count/2));
                var chatEventType = chatEventTypes.ElementAt(rng.Next(0, chatEventTypes.Count));
                return new ChatEvent
                {
                    ChatEventId = Guid.NewGuid(),
                    ChatterId = chatter.ChatterId,
                    ChatEventName = chatEventType.ChatEventName,
                    EventDateTime = new DateTime(2022, 5, 7, 3, 0, 0).AddMinutes(rng.Next(0, 120)),
                    Text = chatEventType.ChatEventName == ChatEventTypes.Comment ? "I'm saying something" : null,
                    Chatter2Id = chatEventType.ChatEventName == ChatEventTypes.HighFive ? chatter2.ChatterId : null,
                };
            }).ToList());

            _dbContext.SaveChanges();
        }


        private readonly List<string> _firstNames = new()
        {
            "John",
            "Lucy",
            "Elle",
            "Matt",
            "Suzy",
            "Mark",
            "Sarah",
            "George",
            "Anne",
            "James",
            "Kate",
            "Andy"
        };
    }
}