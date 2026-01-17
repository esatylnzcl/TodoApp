using Core.Dtos.Task;

namespace Core.IRepositories;
using Entity.Concrete;

public interface ITaskRepository : IBaseRepository<TaskItem>
{
    Task<List<TaskItem>> Filter(TaskFilterDto dto);
}