using System.Threading.Tasks;

namespace EventSourcing
{
    public interface IEntityRepository
    {
        Task<T> Get<T>(string id) where T : EventSourcedEntity;
        Task Save<T>(T entity) where T : EventSourcedEntity;
    }
}