using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing
{
    public class EventSourcedRepository : IEntityRepository
    {
        private readonly IEventStore _store;

        public EventSourcedRepository(IEventStore store)
        {
            _store = store;
        }
        
        public async Task<T> Get<T>(string id) where T : EventSourcedEntity
        {
            var events = await _store.GetEvents(id);
            if (!events.Any())
            {
                throw new Exception("Entity does not exist");
            }

            var ctor = typeof(T)
                .GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Single(c =>
                    c.GetParameters().Length == 2 &&
                    c.GetParameters().First().ParameterType == typeof(string) &&
                    c.GetParameters().Last().ParameterType == typeof(IEnumerable<DomainEvent>));
            
            var instance = (T)ctor.Invoke(new object[] { id, events });

            return instance;
        }

        public async Task Save<T>(T entity) where T : EventSourcedEntity
        {
            await _store.AppendToStream(entity.Id, entity.Version, entity.UncommittedEvents);
            entity.ClearUncommittedEvents();
        }
    }
}