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
        
        private Story(string id, IEnumerable<DomainEvent> pastEvents, string title) : base(id, pastEvents)
        {
            RaiseEvent(new StoryCreated(title,id));
        }
        
        public static Story Create(string title, string entityId)
        {
            return new Story(entityId, new List<DomainEvent>(), title);
        }
        
        public void MoveToInProgress()
        {
            RaiseEvent(new StoryMovedToInProgress(this.Id));
        }

        public void MoveToInReview()
        {
            RaiseEvent(new StoryMovedToInReview(this.Id));
        }

        public void MoveToReadyForDeployment()
        {
            RaiseEvent(new StoryMovedToReadyForDeployment(this.Id));
        }

        public void MoveToDone()
        {
            RaiseEvent(new StoryMovedToDone(this.Id));
        }

        public void Delete()
        {
            RaiseEvent(new StoryDeleted(this.Id));
        }
    }
}