using System;
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

        public Task<IEnumerable<(string eventData, string clrType)>> GetEvents(string id)
        {
            string[] lines = File.ReadAllLines(_filePath);

            var wrappedEvents = lines.Select(this.DeserializeEvent).ToList();
            
            var events = wrappedEvents
                .Where(x => x.Id == id)
                .OrderBy(x => x.Version)
                .Select(x => (x.Event, x.ClrType))
                .AsEnumerable();

            return Task.FromResult(events);
        }

        public Task AppendToStream(string id, int expectedVersion, ICollection<(string eventData, string clrType)> events)
        {
            var actualVersion = GetLatestStoredVersionNumber(id);

            if (actualVersion != expectedVersion)
            {
                throw new AppendOnlyStoreConcurrencyException(expectedVersion, actualVersion);
            }

            var lines = events.Select((x, i) => new WrappedEvent
                {
                    Id = id,
                    Version = expectedVersion + i + 1,
                    Event = x.eventData,
                    ClrType = x.clrType
                })
                .Select(this.SerializeEvent)
                .ToList();
            
            File.AppendAllLines(this._filePath, lines);
            return Task.CompletedTask;
        }

        private int GetLatestStoredVersionNumber(string id)
        {
            var wrappedEvents = File.ReadAllLines(_filePath)
                .Select(JsonConvert.DeserializeObject<WrappedEvent>)
                .Where(x => x.Id == id)
                .ToList();

            if (wrappedEvents.Any())
            {
                return wrappedEvents.Max(x => x.Version);
            }
            
            return 0;
        }

        private WrappedEvent DeserializeEvent(string arg)
        {
            var wrappedEvent = JsonConvert.DeserializeObject<WrappedEvent>(arg);
            return wrappedEvent;
        }

        private string SerializeEvent(WrappedEvent @wrappedEvent)
        {
            return JsonConvert.SerializeObject(wrappedEvent);
        }

        public class WrappedEvent
        {
            public WrappedEvent()
            {
            }

            public string Id { get; set; }
            
            public string Event { get; set; }

            public string ClrType { get; set; }

            public int Version { get; set; }
        }
    }
}