using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VillustraTask.Api.Interfaces;
using VillustraTask.Api.Models;

namespace VillustraTask.Api.Controllers
{
    [Authorize] // Ensure all endpoints require authentication
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository taskRepository, IEmailService emailService, ILogger<TaskController> logger)
        {
            _taskRepository = taskRepository;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new task and send an email notification.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (task == null)
                return BadRequest(new { message = "Task data is required." });

            if (string.IsNullOrEmpty(task.TaskName))
                return BadRequest(new { message = "Task Name is required." });

            if (string.IsNullOrEmpty(task.AssignedTo))
                return BadRequest(new { message = "AssignedTo field is required." });

            try
            {
                var result = await _taskRepository.InsertTaskAsync(task);
                if (result > 0)
                {
                    // Format email
                    var subject = $"[New Task] {task.TaskName} Assigned to You";
                    var body = $@"
                        <p>Hello,</p>
                        <p>You have been assigned a new task:</p>
                        <ul>
                            <li><strong>Task Name:</strong> {task.TaskName}</li>
                            <li><strong>Description:</strong> {task.TaskDescription}</li>
                            <li><strong>Priority:</strong> {task.TaskPriority}</li>
                            <li><strong>Status:</strong> {task.TaskStatus}</li>
                        </ul>
                        <p>Regards,<br/>VillustraTask Team</p>";

                    await _emailService.SendEmailAsync(task.AssignedTo, subject, body);

                    return Ok(new { message = "Task created and email sent successfully." });
                }

                _logger.LogWarning("Task creation failed for {TaskName}.", task.TaskName);
                return BadRequest(new { message = "Error creating task. Check logs for details." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in CreateTask: {Message}", ex.Message);
                return StatusCode(500, new { message = "Internal server error. Please try again later." });
            }
        }

        /// <summary>
        /// Get a list of all tasks.
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Update a task.
        /// </summary>
        [HttpPut("update")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskItem task)
        {
            if (task == null || task.TaskId <= 0)
            {
                _logger.LogWarning("Invalid task data: {Task}", task);
                return BadRequest(new { message = "Invalid task data." });
            }

            try
            {
                var existingTask = await _taskRepository.GetTaskByIdAsync(task.TaskId);
                if (existingTask == null)
                {
                    _logger.LogWarning("Task not found: {TaskId}", task.TaskId);
                    return NotFound(new { message = "Task not found." });
                }

                var result = await _taskRepository.UpdateTaskAsync(task);
                if (result > 0)
                {
                    return Ok(new { message = "Task updated successfully." });
                }

                _logger.LogWarning("Failed to update Task ID: {TaskId}", task.TaskId);
                return BadRequest(new { message = "Error updating task. Check logs for details." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in UpdateTask: {Message}", ex.Message);
                return StatusCode(500, new { message = "Internal server error." });
            }
        }
        /// <summary>
        /// Delete a task.
        /// </summary>
        [HttpDelete("delete/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            if (taskId <= 0)
                return BadRequest(new { message = "Invalid task ID." });

            try
            {
                var existingTask = await _taskRepository.GetTaskByIdAsync(taskId);
                if (existingTask == null)
                    return NotFound(new { message = "Task not found." });

                var result = await _taskRepository.DeleteTaskAsync(taskId);
                if (result > 0)
                {
                    return Ok(new { message = "Task deleted successfully." });
                }

                _logger.LogWarning("Failed to delete Task ID: {TaskId}", taskId);
                return BadRequest(new { message = "Error deleting task." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in DeleteTask: {Message}", ex.Message);
                return StatusCode(500, new { message = "Internal server error." });
            }
        }
        // GetTaskById
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("Task not found: {TaskId}", taskId);
                return NotFound(new { message = "Task not found." });
            }

            return Ok(task);
        }
    }
}
