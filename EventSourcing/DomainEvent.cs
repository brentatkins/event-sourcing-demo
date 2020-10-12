using System;

namespace EventSourcing
{
    public class DomainEvent
    {
        public DomainEvent(string entityId, string userId)
        {
            UserId = userId;
            TimeStamp = DateTime.UtcNow;
            EntityId = entityId;
        }
        
        public DateTime TimeStamp { get; }

        public string UserId { get; }
        
        public string EntityId { get; internal set; }
    }
}