using Core.Dtos.Task;
using Core.IRepositories;
using DataAccess.Context;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

/*
 * 2 tane bağımsız
 * (Yani tek bi apide bu 2 entity'i aynı anda etkileyecek bir durum api,fonksiyon olmadığından dolayı bağımsız diyorum.)
 * bu yüzden ayrı ayrı repository açıp, rollback , commit gibi fonkların direk implemente edildiği unitofwork yapısı kullanmadım.
 * bence overengineering olurdu
 */

public class TaskRepository : BaseRepository<TaskItem> , ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context)
    {
    }
    
    public new async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    // Aşağıdaki yapı bence multirollü yapılarda falan daha esnek ve kontrol edilebilir oluyor (bu projede olmasa da ekledim) o yüzden LINQ'deki o "standartlaşmış" sorguyu kullanmadım.
    public async Task<List<TaskItem>> Filter(TaskFilterDto filterDto)
    {
     IQueryable<TaskItem> query = _context.Tasks.Include(t => t.Category);

     if (!string.IsNullOrEmpty(filterDto.Title))
      query = query.Where(x => x.Title.Contains(filterDto.Title));

     if (filterDto.UserId.HasValue)
      query = query.Where(x => x.UserId == filterDto.UserId.Value);

     if (filterDto.IsDeleted.HasValue)
      query = query.Where(x => x.IsDeleted == filterDto.IsDeleted.Value);

     if (filterDto.Status.HasValue)
      query = query.Where(x => x.Status == filterDto.Status.Value);

     if (filterDto.CategoryId.HasValue)
      query = query.Where(x => x.CategoryId == filterDto.CategoryId.Value);

     if (filterDto.StartDate.HasValue)
      query = query.Where(x => x.StartDate >= filterDto.StartDate.Value);

     if (filterDto.EndDate.HasValue)
      query = query.Where(x => x.EndDate <= filterDto.EndDate.Value);

     return await query.ToListAsync();
    }
}