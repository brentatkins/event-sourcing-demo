namespace EventSourcing.EventBus
{
    public interface IEventBus
    {
        void Publish(DomainEvent @event);
    }
}