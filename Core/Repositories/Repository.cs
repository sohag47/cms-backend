using Core.Data;
using Core.Models.Base;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System.Linq.Expressions;

namespace Core.Repositories;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? includes = null)
    {
        IQueryable<T> query = _dbSet;
        if (includes != null)
            query = includes(query);

        return await query.ToListAsync();
    }

    public async Task<PagedResult<T>> GetPagedAsync(
        int page,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includes = null)
    {
        IQueryable<T> query = _dbSet;

        if (includes != null)
            query = includes(query);

        if (filter != null)
            query = query.Where(filter);

        var totalCount = await query.CountAsync();

        if (orderBy != null)
            query = orderBy(query);

        var data = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>
        {
            Total = totalCount,
            Page = page,
            PageSize = pageSize,
            Data = data
        };
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }



    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
