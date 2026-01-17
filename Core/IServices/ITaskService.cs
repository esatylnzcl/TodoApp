using Core.Dtos.Task;
using Core.Results;
using Entitiy.Enum;

namespace Core.IServices;

public interface ITaskService
{
    Task<DataResponse<List<TaskDto>>> GetUserTasksAsync(int userId);
    Task<DataResponse<TaskDto>> GetTaskByIdAsync(int taskId, int userId);
    Task<DataResponse<TaskDto>> CreateTaskAsync(TaskCreateDto taskCreateDto, int userId);
    Task<Response> UpdateTaskAsync(TaskUpdateDto taskUpdateDto, int userId);
    Task<Response> DeleteTaskAsync(int taskId, int userId);
}