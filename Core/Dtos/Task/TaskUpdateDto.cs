using Entitiy.Enum;

namespace Core.Dtos.Task;

public class TaskUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } 
    public TaskStatusEnum Status { get; set; }
    public int CategoryId { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
}