using System;
using System.Collections.Generic;

namespace EventSourcing
{
    public class EventStoreConcurrencyException : Exception
    {
        public EventStoreConcurrencyException(int expectedVersion, int actualVersion, IEnumerable<DomainEvent> events)
        {
            ExpectedVersion = expectedVersion;
            ActualVersion = actualVersion;
            Events = events;
        }

        public int ExpectedVersion { get; }
        
        public int ActualVersion { get; }
        
        public IEnumerable<DomainEvent> Events { get; }
    }
}