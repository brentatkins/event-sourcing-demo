using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.EventBus;

namespace EventSourcing.Projections
{
    public abstract class LiveProjection<TState> : IDomainEventHandler
    {
        public TState State { get; private set; }

        public LiveProjection()
        {
            State = GetEmptyState();
        }

        public void Hydrate(IEnumerable<DomainEvent> events)
        {
            this.State = this.Replay(events);
        }
        
        protected abstract TState Handle(TState state, DomainEvent @event);

        protected abstract TState GetEmptyState();

        private TState Replay(IEnumerable<DomainEvent> events)
        {
            return events.Aggregate(this.GetEmptyState(), this.Handle);
        }
        
        public void Handle(DomainEvent @event)
        {
            this.State = this.Handle(this.State, @event);
        }
    }
}