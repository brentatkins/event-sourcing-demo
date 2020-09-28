using System;

namespace EventSourcing
{
    public class BaseEvent : IEvent
    {
        public BaseEvent(string userId)
        {
            UserId = userId;
            TimeStamp = DateTime.UtcNow;
        }
        
        public DateTime TimeStamp { get; }

        public string UserId { get; }
    }
}