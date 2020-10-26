using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSourcing.Projections
{
    public class InMemoryDtoRepository : IDtoRepository
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Dto>> _items =
            new ConcurrentDictionary<Type, ConcurrentDictionary<string, Dto>>();

        public Task<IEnumerable<T>> All<T>() where T : Dto
        {
            var items = _items[typeof(T)].Select(x => (T)x.Value);

            return Task.FromResult(items);
        }
        
        public Task<T?> GetById<T>(string id) where T : Dto
        {
            if (_items.ContainsKey(typeof(T)))
            {
                var item = _items[typeof(T)]
                    .Select(x => (T)x.Value)
                    .SingleOrDefault(x => x.Id == id);

                return Task.FromResult(item);
            }

            return Task.FromResult<T?>(null);
        }

        public Task<IEnumerable<T>> Find<T>(Expression<Func<T, bool>> expression) where T : Dto
        {
            var items = _items[typeof(T)]
                .Select(x => (T)x.Value)
                .Where(expression.Compile());
            
            return Task.FromResult(items);
        }

        public Task Insert<T>(T obj) where T : Dto
        {
            if (!_items.ContainsKey(typeof(T)))
            {
                _items.TryAdd(typeof(T), new ConcurrentDictionary<string, Dto>());
            }

            _items[typeof(T)].TryAdd(obj.Id, obj);

            return Task.CompletedTask;
        }

        public Task Update<T>(T obj, string id) where T : Dto
        {
            if (!_items.ContainsKey(typeof(T)))
            {
                _items.TryAdd(typeof(T), new ConcurrentDictionary<string, Dto>());
            }
            
            _items[typeof(T)][id] = obj;
            
            return Task.CompletedTask;
        }
    }
}