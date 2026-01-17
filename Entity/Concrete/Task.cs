using Entitiy.Enum;
using TaskStatusEnum = Entitiy.Enum.TaskStatusEnum; // Thread kütüphanesine ref veriyor yoksa TaskStatusEnum diye isimlendirirsekte çözülür.

namespace Entity.Concrete;

public class TaskItem : BaseEntitiy
{
    public string Title { get; set; } 
    public string Description { get; set; }
    public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
    public DateTime EndDate { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}