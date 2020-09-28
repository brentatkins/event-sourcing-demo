using System;
using System.Collections.Generic;

namespace EventSourcing
{
    public class EventStoreConcurrencyException : Exception
    {
        public int ExpectedVersion { get; }
        public int ActualVersion { get; }
        public IEnumerable<IEvent> ExistingEvents { get; }

        public EventStoreConcurrencyException(int expectedVersion, int actualVersion, IEnumerable<IEvent> existingEvents)
        {
            ExpectedVersion = expectedVersion;
            ActualVersion = actualVersion;
            ExistingEvents = existingEvents;
        }
    }
}