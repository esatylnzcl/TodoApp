using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Task;

public class TaskCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 1000 characters")]
    public string Description { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a valid positive number")]
    public int CategoryId { get; set; }
    
    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }
    
    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }
}