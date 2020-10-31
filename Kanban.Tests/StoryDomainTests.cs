using System;
using EventSourcing.Tests.TestHelpers;
using Kanban.Events;
using Xunit;

namespace Kanban.Tests
{
    public class StoryDomainTests : DomainTestBase<Story>
    {
        [Fact]
        public void CreateStory_StoryIsCreated()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given()
                .When(() => Story.Create(title, entityId))
                .Then(new StoryCreated(title, entityId));
        }

        [Fact]
        public void MoveToInProgress_StoryMovedToInProgress()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId))
                .When((story) => story.MoveToInProgress())
                .Then(new StoryMovedToInProgress(entityId));
        }
        
        [Fact]
        public void MoveToInReview_StoryMovedToInReview()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId),
                new StoryMovedToInProgress(entityId))
                .When((story) => story.MoveToInReview())
                .Then(new StoryMovedToInReview(entityId));
        }
        
        [Fact]
        public void MoveToReadyForDeployment_StoryMovedToReadyForDeployment()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId),
                    new StoryMovedToInProgress(entityId),
                    new StoryMovedToInReview(entityId))
                .When((story) => story.MoveToReadyForDeployment())
                .Then(new StoryMovedToReadyForDeployment(entityId));
        }
        
        [Fact]
        public void MoveToDone_StoryMovedToDone()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId),
                    new StoryMovedToInProgress(entityId),
                    new StoryMovedToInReview(entityId),
                    new StoryMovedToReadyForDeployment(entityId))
                .When((story) => story.MoveToDone())
                .Then(new StoryMovedToDone(entityId));
        }
        
        [Fact]
        public void DeleteStory_ShouldDeleteStory()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId))
                .When(story => story.Delete())
                .Then(new StoryDeleted(entityId));
        }
    }
}