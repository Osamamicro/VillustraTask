using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VillustraTask.Api.Interfaces;
using VillustraTask.Api.Models;

namespace VillustraTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmailService _emailService;

        public TaskController(ITaskRepository taskRepository, IEmailService emailService)
        {
            _taskRepository = taskRepository;
            _emailService = emailService;
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (task == null)
                return BadRequest(new { message = "Invalid task data." });

            var result = await _taskRepository.InsertTaskAsync(task);
            if (result > 0)
            {
                var subject = $"New Task Assigned: {task.TaskName}";
                var body = $"<p>You have been assigned a new task: <strong>{task.TaskName}</strong></p>" +
                           $"<p>Description: {task.TaskDescription}</p>";

                await _emailService.SendEmailAsync(task.AssignedTo, subject, body);

                return Ok(new { message = "Task created and email sent successfully." });
            }

            return BadRequest(new { message = "Error creating task. Check logs for details." });
        }
    }
}
