using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EventSourcing;
using EventSourcing.Projections;
using Kanban.Events;

namespace Kanban.Projections
{
    public class KanbanBoardLiveProjection : LiveProjection<KanbanBoardDto>
    {
        public KanbanBoardLiveProjection(IEnumerable<DomainEvent> events) : base(events)
        {
        }

        protected override KanbanBoardDto Handle(KanbanBoardDto state, DomainEvent @event)
        {
            return @event switch
            {
                StoryCreated storyCreated => state with{ Backlog =
                    state.Backlog.Add(new StoryDto(storyCreated.EntityId, storyCreated.Title))},

                StoryMovedToInProgress storyMovedToInProgress => this.MoveToInProgress(state, storyMovedToInProgress),

                StoryMovedToInReview storyMovedToInReview => this.MoveToInReview(state, storyMovedToInReview),

                StoryMovedToReadyForDeployment storyMovedToReadyForDeployment => this.MoveToReadyForDevelopment(state, storyMovedToReadyForDeployment),

                StoryMovedToDone storyMovedToDone => this.MoveToDone(state, storyMovedToDone),

                _ => state
            };
        }

        private KanbanBoardDto MoveToInProgress(KanbanBoardDto state, StoryMovedToInProgress storyMovedToInProgress)
        {
            var backlogStory = state.Backlog.Single(dto => dto.Id == storyMovedToInProgress.EntityId);
            
            return state with {
                Backlog = state.Backlog.Remove(backlogStory),
                InProgress = state.InProgress.Add(backlogStory)
                };
        }

        private KanbanBoardDto MoveToInReview(KanbanBoardDto state, StoryMovedToInReview storyMovedToInReview)
        {
            var story = state.Backlog.Single(dto => dto.Id == storyMovedToInReview.EntityId);
            
            return state with {
                InProgress = state.InProgress.Remove(story),
                UnderReview = state.UnderReview.Add(story)
                };
        }

        private KanbanBoardDto MoveToReadyForDevelopment(KanbanBoardDto state, StoryMovedToReadyForDeployment storyMovedToReadyForDeployment)
        {
            var story = state.UnderReview.Single(dto => dto.Id == storyMovedToReadyForDeployment.EntityId);
            
            return state with {
                UnderReview = state.UnderReview.Remove(story),
                ReadForDeployment = state.ReadForDeployment.Add(story)
                };
        }

        private KanbanBoardDto MoveToDone(KanbanBoardDto state, StoryMovedToDone storyMovedToDone)
        {
            var story = state.ReadForDeployment.Single(dto => dto.Id == storyMovedToDone.EntityId);
            
            return state with {
                ReadForDeployment = state.ReadForDeployment.Remove(story),
                Done = state.Done.Add(story)
                };
        }


        protected override KanbanBoardDto GetEmptyState()
        {
            return new KanbanBoardDto()
            {
                Backlog = ImmutableArray<StoryDto>.Empty,
                InProgress = ImmutableArray<StoryDto>.Empty,
                UnderReview = ImmutableArray<StoryDto>.Empty,
                ReadForDeployment = ImmutableArray<StoryDto>.Empty,
                Done = ImmutableArray<StoryDto>.Empty
            };
        }
    }

    public record KanbanBoardDto
    {
        public ImmutableArray<StoryDto> Backlog { get; set; }

        public ImmutableArray<StoryDto> InProgress { get; set; }

        public ImmutableArray<StoryDto> UnderReview { get; set; }

        public ImmutableArray<StoryDto> ReadForDeployment { get; set; }

        public ImmutableArray<StoryDto> Done { get; set; }
    }

    public record StoryDto(string Id, string Title);
}