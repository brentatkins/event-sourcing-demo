using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventSourcing.EventBus;
using FluentAssertions;
using Xunit;

namespace EventSourcing.Tests
{
    public class EventStoreTests
    {
        private readonly string _testDbFilePath;

        public EventStoreTests()
        {
            _testDbFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestDb.txt");
            
            // clear database
            File.WriteAllText(_testDbFilePath, string.Empty);
        }
        
        [Fact]
        public async Task GetEvents_NoneExists_ReturnsNoEvents()
        {
            var sut = this.CreateSut();

            var events = await sut.GetEvents(Guid.NewGuid().ToString());

            events.Should().BeEmpty();
        }
        
        [Fact]
        public async Task AddEvents_NoneExist_ShouldSaveEvents()
        {
            var sut = this.CreateSut();

            var id = Guid.NewGuid().ToString();
            var newEvent = new TestEventA(id, "user", 100);
            await sut.AppendToStream(id, 0, new List<DomainEvent> {newEvent});
            
            var events = (await sut.GetEvents(id)).ToList();

            events.Should().HaveCount(1);
        }
        
        [Fact]
        public async Task AddEvents_OthersExist_ShouldAppendEventToEnd()
        {
            var sut = this.CreateSut();
        
            var id = Guid.NewGuid().ToString();
            var event1 = new TestEventA(id, "user", 100);
            var event2 = new TestEventA(id, "user", 200);
            await sut.AppendToStream(id, 0, new List<DomainEvent> {event1, event2});
            
            var newEvent = new TestEventA(id, "user", 300);
            await sut.AppendToStream(id, 2, new List<DomainEvent> {newEvent});
            
            var events = (await sut.GetEvents(id)).ToList();
        
            events.Should().HaveCount(3);
            var lastEvent = events.Last();
            lastEvent.Should().BeAssignableTo<TestEventA>();
            lastEvent.As<TestEventA>().SequenceNumber.Should().Be(300);
        }
        
        [Fact]
        public async Task AddEvents_IncorrectVersionNumber_ShouldThrow()
        {
            var sut = this.CreateSut();
        
            var id = Guid.NewGuid().ToString();
            var event1 = new TestEventA(id, "user", 100);
            var event2 = new TestEventA(id, "user", 200);
            await sut.AppendToStream(id, 0, new List<DomainEvent> {event1, event2});
            
            var newEvent = new TestEventA(id, "user", 300);
            
            Func<Task> act = async () => await sut.AppendToStream(id, 0, new List<DomainEvent> {newEvent});

            act.Should().Throw<EventStoreConcurrencyException>()
                .Where(x => x.ActualVersion == 2)
                .Where(x => x.ExpectedVersion == 0)
                .Where(x => x.Events.Count() == 2);
        }
        
        [Fact]
        public async Task AddEvents_ShouldAlsoPublishToEventBus()
        {
            var testHandler = new TestDomainEventHandler();
            var sut = this.CreateSut(testHandler);

            var id = Guid.NewGuid().ToString();
            var newEvent = new TestEventA(id, "user", 100);
            await sut.AppendToStream(id, 0, new List<DomainEvent> {newEvent});

            testHandler.HandledEvents.Should().Contain(newEvent);
        }

        [Fact]
        public async Task GetEvents_NoneForThisAggregateButHasOthers_ShouldReturnNothing()
        {
            var sut = this.CreateSut();
        
            var id = Guid.NewGuid().ToString();
            var event1 = new TestEventA(id, "user", 100);
            var event2 = new TestEventA(id, "user", 200);
            await sut.AppendToStream(id, 0, new List<DomainEvent> {event1, event2});
            
            var idWithNoEvents = Guid.NewGuid().ToString();
            var events = (await sut.GetEvents(idWithNoEvents)).ToList();
        
            events.Should().HaveCount(0);
        }
        
        [Fact]
        public async Task GetEvents_EventsExists_ShouldReturnAllEvents()
        {
            var sut = this.CreateSut();
        
            var id = Guid.NewGuid().ToString();
            var event1 = new TestEventA(id, "user", 100);
            var event2 = new TestEventA(id, "user", 200);
            await sut.AppendToStream(id, 0, new List<DomainEvent> {event1, event2});
            
            var events = (await sut.GetEvents(id)).ToList();
        
            events.Should().HaveCount(2);
        }
        
        [Fact]
        public async Task GetEvents_ExistsForMultipleThisAggregates_ShouldReturnOnlyForThisAggregate()
        {
            var sut = this.CreateSut();
        
            var id = Guid.NewGuid().ToString();
            var event1 = new TestEventA(id, "user", 100);
            var event2 = new TestEventA(id, "user", 200);
            await sut.AppendToStream(id, 0, new List<DomainEvent> {event1, event2});
            
            var otherId = Guid.NewGuid().ToString();
            var otherEvent1 = new TestEventA(otherId , "user", 300);
            await sut.AppendToStream(otherId, 0, new List<DomainEvent> {otherEvent1});
            
            var events = (await sut.GetEvents(id)).ToList();
        
            events.Should().HaveCount(2);
        }

        private IEventStore CreateSut(IDomainEventHandler? testEventHandler = null)
        {
            var eventBus = new EventBus.EventBus();
            if (testEventHandler is not null)
            {
                eventBus.Subscribe(testEventHandler);
            }
            return new EventStore(new PoorMansAppendOnlyStore(_testDbFilePath), eventBus);
        }

        public class TestEventA : DomainEvent
        {
            public int SequenceNumber { get; }

            public TestEventA(string entityId, string userId, int sequenceNumber) : base(entityId, userId)
            {
                SequenceNumber = sequenceNumber;
            }
        }

        public class TestDomainEventHandler : IDomainEventHandler
        {
            public List<DomainEvent> HandledEvents { get; private set; }

            public TestDomainEventHandler()
            {
                HandledEvents = new List<DomainEvent>();
            }
            
            public void Handle(DomainEvent @event)
            {
                HandledEvents.Add(@event);
            }
        }
    }
}