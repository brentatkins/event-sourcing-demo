using EventSourcing;

namespace Kanban.Events
{
    public class StoryCreated : DomainEvent
    {
        public string Title { get; }

        public StoryCreated(string title, string entityId, string userId) : base(entityId, userId)
        {
            Title = title;
        }
    }
    
    public class StoryMovedToInProgress : DomainEvent
    {
        public StoryMovedToInProgress(string entityId, string userId) : base(entityId, userId)
        {
        }
    }

    public class StoryMovedToInReview : DomainEvent
    {
        public StoryMovedToInReview(string entityId, string userId) : base(entityId, userId)
        {
        }
    }

    public class StoryMovedToReadyForDeployment : DomainEvent
    {
        public StoryMovedToReadyForDeployment(string entityId, string userId) : base(entityId, userId)
        {
        }
    }
    
    public class StoryMovedToDone : DomainEvent
    {
        public StoryMovedToDone(string entityId, string userId) : base(entityId, userId)
        {
        }
    }
}