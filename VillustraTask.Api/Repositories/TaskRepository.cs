using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VillustraTask.Api.Interfaces;
using VillustraTask.Api.Models;

namespace VillustraTask.Api.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<int> InsertTaskAsync(TaskItem task)
        {
            using var connection = CreateConnection();
            var query = @"EXEC dbo.sp_InsertTask 
                          @TaskName, @TaskDescription, @TaskFile, 
                          @AssignedTo, @TaskStatus, @TaskPriority, 
                          @CreatedBy";
            return await connection.ExecuteAsync(query, task);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            using var connection = CreateConnection();
            var query = "EXEC dbo.sp_GetAllTasks";
            return await connection.QueryAsync<TaskItem>(query);
        }

        public async Task<TaskItem> GetTaskByIdAsync(int taskId)
        {
            using var connection = CreateConnection();
            var query = "EXEC dbo.sp_GetTaskById @TaskId";
            return await connection.QueryFirstOrDefaultAsync<TaskItem>(query, new { TaskId = taskId });
        }
    }
}
