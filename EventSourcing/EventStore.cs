using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventSourcing
{
    public class EventStore : IEventStore
    {
        private readonly IAppendOnlyStore _store;

        public EventStore(IAppendOnlyStore store)
        {
            _store = store;
        }

        public async Task<IEnumerable<IDomainEvent>> GetEvents(string id)
        {
            var eventData = await _store.GetEvents(id);
            var events = eventData.Select(this.DeserializeEvent);
            
            return events;
        }

        public async Task AppendToStream(string id, int expectedVersion, ICollection<IDomainEvent> events)
        {
            var eventData = events
                .Select(this.SerializeEvent)
                .ToList();

            try
            {
                await this._store.AppendToStream(id, expectedVersion, eventData);
            }
            catch (AppendOnlyStoreConcurrencyException ex)
            {
                var pastEvents = await this.GetEvents(id);
                throw new EventStoreConcurrencyException(ex.ExpectedVersion, ex.ActualVersion, pastEvents);
            }
        }
        
        private IDomainEvent DeserializeEvent(string eventData)
        {
            var wrappedEvent = JsonConvert.DeserializeObject<WrappedEvent>(eventData);

            return wrappedEvent.ToDomainEvent();
        }

        private string SerializeEvent(IDomainEvent @event)
        {
            var wrappedEvent = new WrappedEvent(@event);

            return JsonConvert.SerializeObject(wrappedEvent);
        }
        
        public class WrappedEvent
        {
            public WrappedEvent() { }
            
            public WrappedEvent(IDomainEvent @event)
            {
                ClrType = @event.GetType().AssemblyQualifiedName;
                Event = JsonConvert.SerializeObject(@event);
            }
            
            public string Event { get; set; }

            public string ClrType { get; set; }

            public IDomainEvent ToDomainEvent()
            {
                var type = Type.GetType(this.ClrType, true);
                var @event = (IDomainEvent)JsonConvert.DeserializeObject(this.Event, type);

                return @event;
            }
        }
    }
}