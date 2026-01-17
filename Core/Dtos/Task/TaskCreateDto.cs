namespace Core.Dtos.Task;

public class TaskCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }
}