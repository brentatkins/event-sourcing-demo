using System.Collections.Generic;
using EventSourcing;
using Kanban.Events;

namespace Kanban
{
    public class Story : EventSourcedEntity
    {
        private Story(string id, IEnumerable<DomainEvent> pastEvents) : base(id, pastEvents)
        {
        }
        
        private Story(string id, IEnumerable<DomainEvent> pastEvents, string title, string userId) : base(id, pastEvents)
        {
            RaiseEvent(new StoryCreated(title,id, userId));
        }
        
        public static Story Create(string title, string entityId, string userId)
        {
            return new Story(entityId, new List<DomainEvent>(), title, userId);
        }
        
        public void MoveToInProgress(string userId)
        {
            RaiseEvent(new StoryMovedToInProgress(this.Id, userId));
        }

        public void MoveToInReview(string userId)
        {
            RaiseEvent(new StoryMovedToInReview(this.Id, userId));
        }

        public void MoveToReadyForDeployment(string userId)
        {
            RaiseEvent(new StoryMovedToReadyForDeployment(this.Id, userId));
        }

        public void MoveToDone(string userId)
        {
            RaiseEvent(new StoryMovedToDone(this.Id, userId));
        }
    }
}