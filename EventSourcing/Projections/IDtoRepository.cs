using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EventSourcing.Projections
{
    public interface IDtoRepository
    {
        Task<IEnumerable<T>> All<T>() where T : Dto;

        Task<IEnumerable<T>> Find<T>(Expression<Func<T, bool>> expression) where T : Dto;

        Task<T?> GetById<T>(string id) where T : Dto;
        
        Task Insert<T>(T obj) where T : Dto;

        Task Update<T>(T obj, string id) where T : Dto;
    }
}