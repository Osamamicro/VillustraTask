using VillustraTask.Api.Models;

namespace VillustraTask.Api.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<int> InsertTaskAsync(TaskItem task);
        Task<TaskItem> GetTaskByIdAsync(int taskId);
        Task<int> UpdateTaskAsync(TaskItem task);
        Task<int> DeleteTaskAsync(int taskId);
    }
}
