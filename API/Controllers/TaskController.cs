using Entity.Concrete;

namespace API.Controllers;
using System.Security.Claims;
using Core.Dtos.Task;
using Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _taskService.GetUserTasksAsync(CurrentUserId);
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _taskService.GetTaskByIdAsync(id, CurrentUserId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskCreateDto dto)
    {
        var result = await _taskService.CreateTaskAsync(dto, CurrentUserId);
        return result.Success ? StatusCode(201, result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(TaskUpdateDto dto)
    {
        var result = await _taskService.UpdateTaskAsync(dto, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id, CurrentUserId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}