using Core.Dtos.Task;
using Core.IRepositories;
using Core.IServices;
using Core.Results;
using Entity.Concrete;


namespace Business.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<DataResponse<List<TaskDto>>> GetUserTasksAsync(int userId)
    {
        var tasks = await _taskRepository.Filter(new TaskFilterDto 
        { 
            UserId = userId, 
            IsDeleted = false 
        });
        
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            EndDate = t.EndDate,
            StartDate = t.StartDate,
            CategoryId = t.CategoryId,
            CategoryName = t.Category.Name,
            CreatedAt = t.CreatedAt
        }).ToList();
        
        return new DataResponse<List<TaskDto>>(true, "Tasks fetched successfully" , taskDtos);
    }

    public async Task<DataResponse<TaskDto>> GetTaskByIdAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task == null || task.UserId != userId || task.IsDeleted)
            return DataResponse<TaskDto>.ErrorDateResponse(null, "Task cannot be found");

        var dto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            StartDate = task.StartDate,
            EndDate = task.EndDate,
            CategoryId = task.CategoryId,
            CategoryName = task.Category.Name,
            CreatedAt = task.CreatedAt
        };

        return DataResponse<TaskDto>.SuccessDataResponse(dto);
    }

    public async Task<DataResponse<TaskDto>> CreateTaskAsync(TaskCreateDto taskCreateDto, int userId)
    {
        var category = await _categoryRepository.GetByIdAsync(taskCreateDto.CategoryId);
        if (category == null || category.IsDeleted)
            return DataResponse<TaskDto>.ErrorDateResponse(null, "Category not found");

        var newT = new TaskItem
        {
            Title = taskCreateDto.Title,
            Description = taskCreateDto.Description,
            StartDate = taskCreateDto.StartDate,
            EndDate = taskCreateDto.EndDate,
            CategoryId = taskCreateDto.CategoryId,
            UserId = userId,
            Status = Entitiy.Enum.TaskStatusEnum.Pending
        };

        await _taskRepository.AddAsync(newT);
        await _taskRepository.SaveAsync();

        var responseDto = new TaskDto { Id = newT.Id, Title = newT.Title };
        return DataResponse<TaskDto>.SuccessDataResponse(responseDto, "Task created succesfully");
    }

    public async Task<Response> UpdateTaskAsync(TaskUpdateDto taskUpdateDto, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskUpdateDto.Id);

        if (task == null || task.UserId != userId || task.IsDeleted)
            return new Response(false, "Task to update could not be found");

        var category = await _categoryRepository.GetByIdAsync(taskUpdateDto.CategoryId);
        if (category == null || category.IsDeleted)
            return new Response(false, "Category not found");

        task.Title = taskUpdateDto.Title;
        task.Description = taskUpdateDto.Description;
        task.Status = taskUpdateDto.Status;
        task.CategoryId = taskUpdateDto.CategoryId;
        task.StartDate = taskUpdateDto.StartDate;
        task.EndDate = taskUpdateDto.EndDate;

        _taskRepository.Update(task);
        // Yukarıdaki non-blocking yapıda await gerektirmiyor.
        await _taskRepository.SaveAsync();

        return new Response(true, "Task updated");
    }

    public async Task<Response> DeleteTaskAsync(int taskId, int userId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);

        if (task == null || task.UserId != userId)
            return new Response(false, "Taks to delete could not be found");

        _taskRepository.SoftDelete(task);
        await _taskRepository.SaveAsync();

        return new Response(true, "Task deleted");
    }
}