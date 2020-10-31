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
            someEntity.DoSomething(true);

            // assert
            someEntity.UncommittedEvents.Select(x => x.EntityId).Should().BeEquivalentTo(entityId);
        }

        public class SomeTestEntity : EventSourcedEntity
        {
            public SomeTestEntity(string id, IEnumerable<DomainEvent> pastEvents) : base(id, pastEvents)
            {
            }

            public void DoSomething(bool value)
            {
                this.RaiseEvent(new SomeTestEventA(this.Id, value));
            }
            
            public void When(SomeTestEventA @event) { }
        }
        
        public class SomeTestEventA : DomainEvent
        {
            public bool SomeProp { get; }

            public SomeTestEventA(string entityId, bool someProp) : base(entityId)
            {
                SomeProp = someProp;
            }
        }
    }
}