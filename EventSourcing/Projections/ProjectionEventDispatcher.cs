using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventSourcing.Projections
{
    public class ProjectionEventDispatcher
    {
        private readonly IDtoRepository _dtoRepository;
        private readonly List<Func<DomainEvent, Task>>_projectionsHandlers = new List<Func<DomainEvent, Task>>();

        public ProjectionEventDispatcher(IDtoRepository dtoRepository)
        {
            _dtoRepository = dtoRepository;
        }

        public ProjectionEventDispatcher RegisterProjection<TDto>(IProjection<TDto> projection) where TDto : Dto
        {
            async Task ProjectEvent(DomainEvent @event)
            {
                var id = @event.EntityId;
                var dto = await _dtoRepository.GetById<TDto>(id);

                var updatedDto = projection.Project(dto, @event);
                if (updatedDto is not null && updatedDto != dto)
                {
                    await _dtoRepository.Update(updatedDto, id);
                }
            }

            _projectionsHandlers.Add(ProjectEvent);

            return this;
        }

        public Task DispatchEvent(DomainEvent @event)
        {
            var tasks = _projectionsHandlers.Select(handler => handler(@event));

            return Task.WhenAll(tasks);
        }
    }
}