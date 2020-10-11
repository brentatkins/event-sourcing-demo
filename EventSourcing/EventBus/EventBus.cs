using System.Collections.Generic;

namespace EventSourcing.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly List<IDomainEventHandler> _handlers
            = new List<IDomainEventHandler>();

        public void Publish(DomainEvent @event)
        {
            foreach (var handler in _handlers)
            {
                handler.Handle(@event);
            }
        }

        public void Subscribe(IDomainEventHandler handler)
        {
            _handlers.Add(handler);
        }
    }
}