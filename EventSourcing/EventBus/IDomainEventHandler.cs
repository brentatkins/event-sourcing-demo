namespace EventSourcing.EventBus
{
    public interface IDomainEventHandler
    {
        void Handle(DomainEvent @event);
    }
}