namespace Entity.Concrete;

public class User : BaseEntitiy
{
    public string Username { get; set; } 
    public string Email { get; set; } 
    public string HashedPassword { get; set; }
    public string FirstName { get; set; } 
    public string LastName { get; set; } 
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    
}