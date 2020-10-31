using System.Collections.Immutable;
using System.Linq;
using EventSourcing;
using EventSourcing.Projections;
using Kanban.Events;

namespace Kanban.Projections
{
    public class KanbanBoardLiveProjection : LiveProjection<KanbanBoardDto>
    {
        protected override KanbanBoardDto Handle(KanbanBoardDto state, DomainEvent @event)
        {
            return @event switch
            {
                StoryCreated storyCreated => state with { Stories =
                    state.Stories.Add((KanbanBoardDto.StoryStatus.Backlog,
                        new StoryDto(storyCreated.EntityId, storyCreated.Title, storyCreated.TimeStamp)))},

                StoryMovedToInProgress storyMovedToInProgress => this.MoveStoryToStatus(state,
                    storyMovedToInProgress.EntityId, KanbanBoardDto.StoryStatus.InProgress),

                StoryMovedToInReview storyMovedToInReview => this.MoveStoryToStatus(state,
                    storyMovedToInReview.EntityId, KanbanBoardDto.StoryStatus.UnderReview),

                StoryMovedToReadyForDeployment storyMovedToReadyForDeployment => this.MoveStoryToStatus(state,
                    storyMovedToReadyForDeployment.EntityId, KanbanBoardDto.StoryStatus.ReadyForDeployment),

                StoryMovedToDone storyMovedToDone => this.MoveStoryToStatus(state, storyMovedToDone.EntityId,
                    KanbanBoardDto.StoryStatus.Done),

                StoryDeleted storyDeleted => state with { Stories =
                    state.Stories.RemoveAll(s => s.Story.Id == storyDeleted.EntityId)},

                _ => state
            };
        }

        private KanbanBoardDto MoveStoryToStatus(KanbanBoardDto state, string storyId,
            KanbanBoardDto.StoryStatus newStatus)
        {
            var newStories = state.Stories.Select(tuple => tuple switch
                {
                    (_, StoryDto dto) when dto.Id == storyId => (newStatus, dto),
                    _ => tuple
                }
            );

            return state with {Stories = newStories.ToImmutableArray()};
        }

        protected override KanbanBoardDto GetEmptyState()
        {
            return new KanbanBoardDto()
            {
                Stories = ImmutableArray<(KanbanBoardDto.StoryStatus Status, StoryDto Story)>.Empty
            };
        }
    }
}