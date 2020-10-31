using System;

namespace EventSourcing
{
    public abstract class DomainEvent
    {
        protected DomainEvent(string entityId)
        {
            EntityId = entityId;
            TimeStamp = DateTime.UtcNow;
        }

        public DateTime TimeStamp { get; set; }

        public string EntityId { get; set; }
    }
}