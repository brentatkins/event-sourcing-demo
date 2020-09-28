using System;

namespace EventSourcing
{
    public class BaseDomainEvent : IDomainEvent
    {
        public BaseDomainEvent(string userId)
        {
            UserId = userId;
            TimeStamp = DateTime.UtcNow;
        }
        
        public DateTime TimeStamp { get; }

        public string UserId { get; }
    }
}