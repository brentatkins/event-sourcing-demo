using EventSourcing;

namespace Kanban.Events
{
    public class StoryCreated : DomainEvent
    {
        public string Title { get; }

        public StoryCreated(string title, string entityId) : base(entityId)
        {
            Title = title;
        }
    }
    
    public class StoryMovedToInProgress : DomainEvent
    {
        public StoryMovedToInProgress(string entityId) : base(entityId) { }
    }

    public class StoryMovedToInReview : DomainEvent
    {
        public StoryMovedToInReview(string entityId) : base(entityId) { }
    }

    public class StoryMovedToReadyForDeployment : DomainEvent
    {
        public StoryMovedToReadyForDeployment(string entityId) : base(entityId) { }
    }
    
    public class StoryMovedToDone : DomainEvent
    {
        public StoryMovedToDone(string entityId) : base(entityId) { }
    }

    public class StoryDeleted : DomainEvent
    {
        public StoryDeleted(string entityId) : base(entityId) { }
    }
}