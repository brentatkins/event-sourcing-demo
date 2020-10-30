using System.Collections.Generic;

namespace EventSourcing.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly List<IDomainEventHandler> _handlers
            = new List<IDomainEventHandler>();

        public EventBus(IEnumerable<IDomainEventHandler> handlers)
        {
            foreach (var domainEventHandler in handlers)
            {
                Subscribe(domainEventHandler);
            }
        }
        
        public void Publish(DomainEvent @event)
        {
            foreach (var handler in _handlers)
            {
                handler.Handle(@event);
            }
        }

        private void Subscribe(IDomainEventHandler handler)
        {
            _handlers.Add(handler);
        }
    }
}