using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
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

        private IDbConnection CreateConnection() =>
            new SqlConnection(_connectionString);

        public async Task<int> InsertTaskAsync(TaskItem task)
        {
            using var connection = CreateConnection();
            var query = @"EXEC dbo.sp_InsertTask 
                          @TaskName, @TaskDescription, @TaskFile, @AssignedTo, @TaskStatus, @TaskPriority, @CreatedBy";
            return await connection.ExecuteAsync(query, task);
        }

        public async Task<IEnumerable<TaskItem>> GetTasksAsync()
        {
            using var connection = CreateConnection();
            var query = "EXEC dbo.sp_GetTasks";
            return await connection.QueryAsync<TaskItem>(query);
        }
    }
}
