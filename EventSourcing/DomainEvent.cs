using System;

namespace EventSourcing
{
    public class DomainEvent
    {
        public DomainEvent(string entityId, string userId)
        {
            UserId = userId;
            EntityId = entityId;
        }
        
        public DateTime TimeStamp { get; set; }

        public string UserId { get; }
        
        public string EntityId { get; internal set; }
    }
}