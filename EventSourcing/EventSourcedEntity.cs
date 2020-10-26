using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;

namespace EventSourcing
{
    public abstract class EventSourcedEntity
    {
        private List<DomainEvent> _uncommittedEvents;
        public string Id { get; protected set; }

        internal int Version { get; private set; }
        
        protected EventSourcedEntity(string id, IEnumerable<DomainEvent> pastEvents)
        {
            _uncommittedEvents = new List<DomainEvent>();
            Id = id;
            Version = 0;
            
            foreach (var @event in pastEvents)
            {
                ApplyEvent(@event);
                Version++;
            }
        }

        private void ApplyEvent(DomainEvent @event)
        {
            try
            {
                ((dynamic) this).When((dynamic) @event);
            }
            catch (RuntimeBinderException ex)
            {
                if (!ex.Message.Contains("does not contain a definition for 'When'"))
                {
                    throw;
                }
            }
        }

        protected void RaiseEvent(DomainEvent @event)
        {
            @event.EntityId = Id;
            _uncommittedEvents.Add(@event);
            ApplyEvent(@event);
        }

        internal void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        public ICollection<DomainEvent> UncommittedEvents => _uncommittedEvents.AsReadOnly();
    }
}