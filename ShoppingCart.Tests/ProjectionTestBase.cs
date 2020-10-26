using System;
using System.Runtime.ExceptionServices;
using EventSourcing;
using EventSourcing.Projections;
using FluentAssertions;
using Xunit;

namespace ShoppingCart.Tests
{
    public class ProjectionTestBase<TProjection, TDto> where TProjection : IProjection<TDto> where TDto : Dto
    {
        private readonly IDtoRepository _repo;
        private ProjectionEventDispatcher _eventDispatcher;
        private DomainEvent[] _givenEvents;

        public ProjectionTestBase()
        {
            _repo = new InMemoryDtoRepository();
            _eventDispatcher = new ProjectionEventDispatcher(_repo);
            _givenEvents = new DomainEvent[] { };
        }
        
        protected ProjectionTestBase<TProjection, TDto> Given(params DomainEvent[] events)
        {
            _givenEvents = events;
            return this;
        }

        public void Then(Dto dto)
        {
            DispatchEvents();

            var actual = _repo.GetById<TDto>(dto.Id).Result;
            
            actual.Should().BeEquivalentTo(dto);
        }
        
        public void Then(string id, Action<TDto?> customAssert)
        {
            DispatchEvents();

            var actual = _repo.GetById<TDto>(id).Result;
            
            customAssert(actual);
        }

        private void DispatchEvents()
        {
            var projection = Activator.CreateInstance<TProjection>();

            _eventDispatcher.RegisterProjection(projection);
            
            foreach (var domainEvent in _givenEvents)
            {
                try
                {
                    _eventDispatcher.DispatchEvent(domainEvent).Wait();
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException != null)
                    {
                        ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }
    }
}