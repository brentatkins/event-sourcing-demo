using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace EventSourcing.Tests
{
    public class EventStoreEntityTests
    {
        [Fact]
        public void RaisingEvents_ShouldAttachEntityId()
        {
            // arrange
            var entityId = Guid.NewGuid().ToString();
            var someEntity = new SomeTestEntity(entityId, new List<DomainEvent>());
            
            // act
            someEntity.DoSomething(true, "a user");

            // assert
            someEntity.UncommittedEvents.Select(x => x.EntityId).Should().BeEquivalentTo(entityId);
        }

        public class SomeTestEntity : EventSourcedEntity
        {
            public SomeTestEntity(string id, IEnumerable<DomainEvent> pastEvents) : base(id, pastEvents)
            {
            }

            public void DoSomething(bool value, string userId)
            {
                this.RaiseEvent(new SomeTestEventA(this.Id, userId, value));
            }
            
            public void When(SomeTestEventA @event) { }
        }
        
        public class SomeTestEventA : DomainEvent
        {
            public bool SomeProp { get; }

            public SomeTestEventA(string entityId, string userId, bool someProp) : base(entityId, userId)
            {
                SomeProp = someProp;
            }
        }
    }
}