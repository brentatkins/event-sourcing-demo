using System;
using System.Collections.Generic;
using EventSourcing.Projections;

namespace EventSourcing.Tests.TestHelpers
{
    public class LiveProjectionTest<T, TDto> where T : LiveProjection<TDto>
    {
        private List<DomainEvent> _givenEvents = new List<DomainEvent>();

        public LiveProjectionTest<T, TDto> Given(params DomainEvent[] events)
        {
            _givenEvents.AddRange(events);
            return this;
        }

        public void Then(Action<TDto> assertionFn)
        {
            var projection = Activator.CreateInstance<T>();
            
            projection.Hydrate(_givenEvents);

            assertionFn(projection.State);
        }
    }
}