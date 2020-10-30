using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IAppendOnlyStore
    {
        Task<IEnumerable<string>> GetEvents(string id);
        
        Task<IEnumerable<string>> GetAllEvents();
        
        Task AppendToStream(string id, int expectedVersion, ICollection<string> events);
    }
}