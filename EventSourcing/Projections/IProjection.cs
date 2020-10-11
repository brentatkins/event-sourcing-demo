namespace EventSourcing.Projections
{
    public interface IProjection<T> where T : Dto
    {
        public T? Project(T? state, DomainEvent @event);
    }
}