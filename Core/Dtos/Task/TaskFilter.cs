using Entitiy.Enum;

namespace Core.Dtos.Task;

public class TaskFilterDto
{
    public string? Title { get; set; }
    public int? UserId { get; set; }
    public bool? IsDeleted { get; set; }
    public TaskStatusEnum? Status { get; set; } 
    public int? CategoryId { get; set; }
    public DateTime? StartDate { get; set; } 
    public DateTime? EndDate { get; set; }
}