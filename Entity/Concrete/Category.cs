namespace Entity.Concrete;

public class Category : BaseEntitiy
{
    public string Name { get; set; } 
    public string? Description { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
