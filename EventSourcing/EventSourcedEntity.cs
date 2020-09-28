using System.Collections.Generic;

namespace EventSourcing
{
    public abstract class EventSourcedEntity
    {
        private List<IDomainEvent> _uncommittedEvents;
        public string Id { get; protected set; }

        internal int Version { get; private set; }
        
        protected EventSourcedEntity(string id, IEnumerable<IDomainEvent> pastEvents)
        {
            _uncommittedEvents = new List<IDomainEvent>();
            Id = id;
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
            _uncommittedEvents.Add(@event);
            ApplyEvent(@event);
        }

        internal void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public ICollection<IDomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();
    }
}