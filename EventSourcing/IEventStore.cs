using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IEventStore
    {
        Task<IEnumerable<DomainEvent>> GetEvents(string id);
        
        Task<IEnumerable<DomainEvent>> GetAllEvents();

        Task AppendToStream(string id, int expectedVersion, ICollection<DomainEvent> events);
    }
}