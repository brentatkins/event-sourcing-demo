using System;
using EventSourcing.Tests.TestHelpers;
using Kanban.Events;
using Xunit;

namespace Kanban.Tests
{
    public class StoryDomainTests : DomainTestBase<Story>
    {
        private readonly string userId = "some user id";

        [Fact]
        public void CreateStory_StoryIsCreated()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given()
                .When(() => Story.Create(title, entityId, userId))
                .Then(new StoryCreated(title, entityId, userId));
        }

        [Fact]
        public void MoveToInProgress_StoryMovedToInProgress()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId, userId))
                .When((story) => story.MoveToInProgress(userId))
                .Then(new StoryMovedToInProgress(entityId, userId));
        }
        
        [Fact]
        public void MoveToInReview_StoryMovedToInReview()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId, userId),
                new StoryMovedToInProgress(entityId, userId))
                .When((story) => story.MoveToInReview(userId))
                .Then(new StoryMovedToInReview(entityId, userId));
        }
        
        [Fact]
        public void MoveToReadyForDeployment_StoryMovedToReadyForDeployment()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId, userId),
                    new StoryMovedToInProgress(entityId, userId),
                    new StoryMovedToInReview(entityId, userId))
                .When((story) => story.MoveToReadyForDeployment(userId))
                .Then(new StoryMovedToReadyForDeployment(entityId, userId));
        }
        
        [Fact]
        public void MoveToDone_StoryMovedToDone()
        {
            var entityId = Guid.NewGuid().ToString();
            const string title = "Some story title";

            Given(new StoryCreated(title, entityId, userId),
                    new StoryMovedToInProgress(entityId, userId),
                    new StoryMovedToInReview(entityId, userId),
                    new StoryMovedToReadyForDeployment(entityId, userId))
                .When((story) => story.MoveToDone(userId))
                .Then(new StoryMovedToDone(entityId, userId));
        }
    }
}