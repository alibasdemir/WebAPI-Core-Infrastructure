using Core.Dynamic;
using Core.Pagination;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public interface IAsyncRepository<T> : IQuery<T>
        where T : Entity
    {
        Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task SoftDeleteAsync(T entity);

        Task<IPaginate<T>> GetListAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int index = 0,
            int size = 10,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );

        Task<IPaginate<T>> GetListByDynamicAsync(
            DynamicQuery dynamic,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int index = 0,
            int size = 10,
            bool enableTracking = true,
            CancellationToken cancellationToken = default
        );
    }
}
