namespace Core.IRepositories;
using System.Linq.Expressions;
public interface IBaseRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    
    Task<bool> AddAsync(T entity);
    bool Update(T entity);
    bool SoftDelete(T entity);
    
    Task<int> SaveAsync();
    
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
}