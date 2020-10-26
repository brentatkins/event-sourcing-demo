using System.Collections.Generic;
using System.Linq;
using EventSourcing.EventBus;

namespace EventSourcing.Projections
{
    public abstract class LiveProjection<TState> : IDomainEventHandler
    {
        private TState _state;

        public LiveProjection(IEnumerable<DomainEvent> events)
        {
            this._state = this.Replay(events);
        }
        
        protected abstract TState Handle(TState state, DomainEvent @event);

        protected abstract TState GetEmptyState();

        private TState Replay(IEnumerable<DomainEvent> events)
        {
            return events.Aggregate(this.GetEmptyState(), this.Handle);
        }
        
        public void Handle(DomainEvent @event)
        {
            this._state = this.Handle(_state, @event);
        }
    }
}