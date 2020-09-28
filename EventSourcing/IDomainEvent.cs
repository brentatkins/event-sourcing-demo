using System;

namespace EventSourcing
{
    public interface IDomainEvent
    {
        DateTime TimeStamp { get; }

        string UserId { get; }
    }
}