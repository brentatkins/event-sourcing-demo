using System.Collections.Generic;

namespace EventSourcing
{
    public abstract class EventSourcedEntity
    {
        public string Id { get; protected set; }

        internal int Version { get; private set; }
        
        protected EventSourcedEntity(string id, IEnumerable<IDomainEvent> pastEvents)
        {
            Id = id;
            UncommittedEvents = new List<IDomainEvent>();
            Version = 0;
            
            foreach (var @event in pastEvents)
            {
                ApplyEvent(@event);
                Version++;
            }
        }

        private void ApplyEvent(IDomainEvent @event)
        {
            ((dynamic) this).When((dynamic)@event);
        }

        protected void RaiseEvent(IDomainEvent @event)
        {
            UncommittedEvents.Add(@event);
            ApplyEvent(@event);
        }
        
        internal ICollection<IDomainEvent> UncommittedEvents { get; }
    }
}