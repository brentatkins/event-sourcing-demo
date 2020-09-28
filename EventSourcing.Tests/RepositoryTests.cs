using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace EventSourcing.Tests
{
    public class RepositoryTests
    {
        private readonly EventStore _eventStore;

        public RepositoryTests()
        {
            string testDbFilePath = Path.Combine(Directory.GetCurrentDirectory(), "TestDb.txt");
            _eventStore = new EventStore(new PoorMansAppendOnlyStore(testDbFilePath));
            
            // clear database
            File.WriteAllText(testDbFilePath, string.Empty);
        }

        [Fact]
        public void Get_DoesNotExist_ShouldThrow()
        {
            var sut = this.CreateSut();

            var id = Guid.NewGuid().ToString();

            Func<Task> act = async () => await sut.Get<TestEntity>(id);

            act.Should().Throw<Exception>();
        }
        
        [Fact]
        public async Task Get_HasExistingEvents_ShouldReplayOldEvents()
        {
            var sut = this.CreateSut();

            var id = Guid.NewGuid().ToString();

            await _eventStore.AppendToStream(id, 0, new List<IDomainEvent>
            {
                new TestEvent("user", true),
                new TestEvent("user", true)
            });

            var entity = await sut.Get<TestEntity>(id);
            entity.CountOfEventsReplayed.Should().Be(2);
            // entity.Id.Should().Be(id);
        }
        
        [Fact]
        public async Task Save_ThereAreUncommittedChanges_ShouldBeSaved()
        {
            var sut = this.CreateSut();

            var entity = TestEntity.Create();

            entity.DoSomething();

            await sut.Save(entity);

            var savedEntity = await sut.Get<TestEntity>(entity.Id);
            savedEntity.CountOfEventsReplayed.Should().Be(1);
        }
        
        [Fact]
        public async Task Save_WasPreviouslySaved_ShouldBeSaved()
        {
            var sut = this.CreateSut();

            var entity = TestEntity.Create();

            entity.DoSomething();

            await sut.Save(entity);

            var savedEntity1 = await sut.Get<TestEntity>(entity.Id);
            savedEntity1.CountOfEventsReplayed.Should().Be(1);
            
            savedEntity1.DoSomething();
            
            await sut.Save(savedEntity1);
            
            var savedEntity2 = await sut.Get<TestEntity>(entity.Id);
            savedEntity2.CountOfEventsReplayed.Should().Be(2);
        }

        private EventSourcedRepository CreateSut()
        {
            var repo = new EventSourcedRepository(_eventStore);

            return repo;
        }

        public class TestEntity : EventSourcedEntity
        {
            public int CountOfEventsReplayed { get; private set; }
            
            private TestEntity(string id, IEnumerable<IDomainEvent> pastEvents) : base(id, pastEvents)
            {
            }

            public static TestEntity Create()
            {
                var newEntity = new TestEntity(Guid.NewGuid().ToString(), new List<IDomainEvent>());

                return newEntity;
            }

            public void DoSomething()
            {
                RaiseEvent(new TestEvent("user", true));
            }

            public void When(TestEvent @event)
            {
                CountOfEventsReplayed++;
            }
        }

        public class TestEvent : BaseDomainEvent
        {
            public bool SomeProp { get; }

            public TestEvent(string userId, bool someProp) : base(userId)
            {
                SomeProp = someProp;
            }
        }
    }
}