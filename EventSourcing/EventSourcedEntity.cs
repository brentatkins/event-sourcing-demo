using System.Collections.Generic;

namespace EventSourcing
{
    public abstract class EventSourcedEntity
    {
        public string Id { get; protected set; }

        internal int Version { get; private set; }
        
        protected EventSourcedEntity(string id, IEnumerable<IEvent> pastEvents)
        {
            Id = id;
            UncommittedEvents = new List<IEvent>();
            Version = 0;
            
            foreach (var @event in pastEvents)
            {
                ApplyEvent(@event);
                Version++;
            }
        }

        private void ApplyEvent(IEvent @event)
        {
            ((dynamic) this).When((dynamic)@event);
        }

        protected void RaiseEvent(IEvent @event)
        {
            UncommittedEvents.Add(@event);
            ApplyEvent(@event);
        }
        
        internal ICollection<IEvent> UncommittedEvents { get; }
    }
}