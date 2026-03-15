using cms_backend.Models.Base;
using System.Linq.Expressions;

namespace cms_backend.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> Query();

    Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? includes = null);

    Task<PagedResult<T>> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includes = null);

    Task<T?> GetByIdAsync(int id);

    Task AddAsync(T entity);

    Task AddRangeAsync(IEnumerable<T> entities);

    void Update(T entity);

    void Delete(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

    Task<int> SaveChangesAsync();
}
