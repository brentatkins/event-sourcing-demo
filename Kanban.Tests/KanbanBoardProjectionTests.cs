using EventSourcing.Tests.TestHelpers;
using FluentAssertions;
using Kanban.Events;
using Kanban.Projections;
using Xunit;

namespace Kanban.Tests
{
    public class KanbanBoardProjectionTests : LiveProjectionTest<KanbanBoardLiveProjection, KanbanBoardDto>
    {
        [Fact]
        public void StoryCreated_ShouldCreateStory()
        {
            var title = "some title";
            var entityId = "some id";

            var storyCreateEvent = new StoryCreated(title, entityId);
            
            Given(storyCreateEvent)
                .Then(board =>
                {
                    board.Backlog.Should().HaveCount(1)
                        .And.HaveElementAt(0, new StoryDto(entityId, title, storyCreateEvent.TimeStamp));
                });
        }
        
        [Fact]
        public void StoryDeleted_ShouldDeleteStory()
        {
            var title = "some title";
            var entityId = "some id";

            Given(new StoryCreated(title, entityId),
                    new StoryDeleted(entityId))
                .Then(board =>
                {
                    board.Backlog.Should().HaveCount(0);
                });
        }
    }
}