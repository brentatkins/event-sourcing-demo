using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EventSourcing
{
    public class PoorMansAppendOnlyStore : IAppendOnlyStore
    {
        private readonly string _filePath;

        public PoorMansAppendOnlyStore(string filePath)
        {
            _filePath = filePath;
        }

        public Task<IEnumerable<string>> GetEvents(string id)
        {
            string[] lines = File.ReadAllLines(_filePath);

            var wrappedEvents = lines.Select(this.DeserializeEvent).ToList();
            
            var events = wrappedEvents
                .Where(x => x.Id == id)
                .OrderBy(x => x.Version)
                .Select(x => x.EventData)
                .AsEnumerable();

            return Task.FromResult(events);
        }

        public Task AppendToStream(string id, int expectedVersion, ICollection<string> events)
        {
            var actualVersion = GetLatestStoredVersionNumber(id);

            if (actualVersion != expectedVersion)
            {
                throw new AppendOnlyStoreConcurrencyException(expectedVersion, actualVersion);
            }

            var lines = events.Select((x, i) => new DbEvent(id, expectedVersion + i + 1, x))
                .Select(this.SerializeEvent)
                .ToList();
            
            File.AppendAllLines(this._filePath, lines);
            return Task.CompletedTask;
        }

        private int GetLatestStoredVersionNumber(string id)
        {
            var wrappedEvents = File.ReadAllLines(_filePath)
                .Select(JsonConvert.DeserializeObject<DbEvent>)
                .Where(x => x.Id == id)
                .ToList();

            if (wrappedEvents.Any())
            {
                return wrappedEvents.Max(x => x.Version);
            }
            
            return 0;
        }

        private DbEvent DeserializeEvent(string data)
        {
            return JsonConvert.DeserializeObject<DbEvent>(data);
        }

        private string SerializeEvent(DbEvent dbEvent)
        {
            return JsonConvert.SerializeObject(dbEvent);
        }

        public class DbEvent
        {
            public DbEvent(string id, int version, string eventData)
            {
                Id = id;
                Version = version;
                EventData = eventData;
            }

            public string Id { get; }
            
            public int Version { get; }
            
            public string EventData { get; }
        }
    }
}