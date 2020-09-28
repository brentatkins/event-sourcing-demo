using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IEventStore
    {
        Task<IEnumerable<IDomainEvent>> GetEvents(string id);

        Task AppendToStream(string id, int expectedVersion, ICollection<IDomainEvent> events);
    }
}