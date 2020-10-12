using System;
using System.Collections.Generic;
using System.Linq;
using EventSourcing;
using FluentAssertions;

namespace ShoppingCart.Tests
{
    public class DomainTestBase<T> where T : EventSourcedEntity
    {
        private DomainEvent[] _givenEvents = Array.Empty<DomainEvent>();
        private Func<T>? _whenNewEntity;
        private Action<T>? _whenExistingEntity;

        internal DomainTestBase<T> Given(params DomainEvent[] givenEvents)
        {
            _givenEvents = givenEvents;
            return this;
        }

        internal DomainTestBase<T> When(Func<T> whenNewEntity)
        {
            _whenNewEntity = whenNewEntity;
            return this;
        }
        
        internal DomainTestBase<T> When(Action<T> whenExistingEntity)
        {
            _whenExistingEntity = whenExistingEntity;
            return this;
        }

        internal void Then(params DomainEvent[] expectedEvents)
        {
            var entity = GetEntity();

            entity.UncommittedEvents.ToList()
                .Should()
                .BeEquivalentTo(expectedEvents.ToList(), 
                    cfg => 
                        cfg.RespectingRuntimeTypes()
                            .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
                            .When(info => info.SelectedMemberPath.EndsWith("TimeStamp")));
        }

        private T GetEntity()
        {
            if (_whenNewEntity != null)
            {
                return _whenNewEntity.Invoke();
            }
            else if (_whenExistingEntity != null)
            {
                var ctor = typeof(T)
                    .GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    .Single(c =>
                        c.GetParameters().Length == 2 &&
                        c.GetParameters().First().ParameterType == typeof(string) &&
                        c.GetParameters().Last().ParameterType == typeof(IEnumerable<DomainEvent>));

                var id = _givenEvents.First().EntityId;
                var entity = (T) ctor.Invoke(new object[] {id, _givenEvents.ToList()});

                _whenExistingEntity.Invoke(entity);
                
                return entity;
            }
            else
            {
                throw new Exception("Must assert on sumting");
            }
        }
    }
}