using System;

namespace EventSourcing
{
    public class DomainEvent : IDomainEvent
    {
        public DomainEvent(string userId)
        {
            UserId = userId;
            TimeStamp = DateTime.UtcNow;
        }
        
        public DateTime TimeStamp { get; }

        public string UserId { get; }
    }
}