using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IAppendOnlyStore
    {
        Task<IEnumerable<(string eventData, string clrType)>> GetEvents(string id);
        
        Task AppendToStream(string id, int expectedVersion, ICollection<(string eventData, string clrType)> events);
    }
}