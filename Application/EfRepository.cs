using Domain.Common;
using Domain.Services;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application
{
    public class EfRepository : IRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EfRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> SingleAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class, IBaseEntity
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<List<T>> ListAsync<T>(CancellationToken cancellationToken = default) where T : class, IBaseEntity
        {
            return _dbContext.Set<T>().ToListAsync();
        }

        public async Task<List<T>> ListAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class, IBaseEntity
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IBaseEntity
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class, IBaseEntity
        {
            _dbContext.Update<T>(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
    }
}
