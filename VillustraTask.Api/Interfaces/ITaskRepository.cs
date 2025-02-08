using VillustraTask.Api.Models;

namespace VillustraTask.Api.Interfaces
{
    public interface ITaskRepository
    {
        Task<int> InsertTaskAsync(TaskItem task);
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int taskId);
    }
}
