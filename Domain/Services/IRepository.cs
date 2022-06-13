using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IRepository
    {
        Task<T> SingleAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class, IBaseEntity;
        Task<List<T>> ListAsync<T>(CancellationToken cancellationToken = default) where T : class, IBaseEntity;
        Task<List<T>> ListAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class, IBaseEntity;
        Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IBaseEntity;
        Task<T> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IBaseEntity;
    }
}
