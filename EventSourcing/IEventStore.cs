using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IEventStore
    {
        Task<IEnumerable<IEvent>> GetEvents(string id);

        Task AppendToStream(string id, int expectedVersion, ICollection<IEvent> events);
    }
}