using System.Threading;
using System.Threading.Tasks;
using EventSourcing;
using Kanban.Projections;
using Microsoft.Extensions.Hosting;

namespace SimpleUI
{
    public class HydrateLiveProjection : IHostedService
    {
        private readonly IEventStore _eventStore;
        private readonly KanbanBoardLiveProjection _liveProjection;

        public HydrateLiveProjection(IEventStore eventStore, KanbanBoardLiveProjection liveProjection)
        {
            _eventStore = eventStore;
            _liveProjection = liveProjection;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllEvents();
            _liveProjection.Hydrate(events);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}