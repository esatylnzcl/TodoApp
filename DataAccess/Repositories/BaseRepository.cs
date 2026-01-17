using Microsoft.EntityFrameworkCore;
using Core.IRepositories;
using Entity.Concrete;
using DataAccess.Context;
using System.Linq.Expressions;
namespace DataAccess.Repositories;

/*
 * Performans için olan AsNoTracking gibi methodları hiç eklemedim default EFCore kullandım.
 */

// burada x.IsDeleted != true eklemedim serviste kontrolü sağlanacak. 

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntitiy
{
    protected readonly AppDbContext _context;
    private readonly DbSet<T> _table;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _table = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _table.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _table.ToListAsync();
    }

    public async Task<bool> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _table.AddAsync(entity);
        return true;
    }

    public bool Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _table.Update(entity);
        return true;
    }

    public bool SoftDelete(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        _table.Update(entity);
        return true;
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
    {
        return await _table.AnyAsync(filter);
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}