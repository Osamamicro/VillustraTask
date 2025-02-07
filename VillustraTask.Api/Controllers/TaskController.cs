using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VillustraTask.Api.Interfaces;
using VillustraTask.Api.Models;

namespace VillustraTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protect endpoints with JWT
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/Task
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _taskRepository.GetTasksAsync();
            return Ok(tasks);
        }

        // POST: api/Task
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            var result = await _taskRepository.InsertTaskAsync(task);
            if (result > 0)
            {
                return Ok(new { message = "Task created successfully." });
            }
            return BadRequest(new { message = "Error creating task." });
        }
    }
}
