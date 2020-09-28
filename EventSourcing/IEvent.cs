using System;

namespace EventSourcing
{
    public interface IEvent
    {
        DateTime TimeStamp { get; }

        string UserId { get; }
    }
}