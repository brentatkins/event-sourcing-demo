using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventSourcing
{
    public class PoorMansEventStore : IEventStore
    {
        private readonly IAppendOnlyStore _store;

        public PoorMansEventStore(IAppendOnlyStore store)
        {
            _store = store;
        }

        public async Task<IEnumerable<IEvent>> GetEvents(string id)
        {
            var eventData = await _store.GetEvents(id);
            var events = eventData.Select(x => this.DeserializeEvent(x.eventData, x.clrType));
            
            return events;
        }

        public async Task AppendToStream(string id, int expectedVersion, ICollection<IEvent> events)
        {
            var eventData = events
                .Select(x => (this.SerializeEvent(x), x.GetType().AssemblyQualifiedName))
                .ToList();
            
            await this._store.AppendToStream(id, expectedVersion, eventData);
        }
        
        private IEvent DeserializeEvent(string eventData, string clrType)
        {
            var type = Type.GetType(clrType, true);
            var @event = (IEvent)JsonConvert.DeserializeObject(eventData, type);
            return @event;
        }

        private string SerializeEvent(IEvent @event)
        {
            return JsonConvert.SerializeObject(@event);
        }
    }
}