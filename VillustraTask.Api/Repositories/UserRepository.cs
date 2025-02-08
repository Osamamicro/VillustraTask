using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using VillustraTask.Api.Interfaces;
using VillustraTask.Api.Models;
using Microsoft.Extensions.Logging;

namespace VillustraTask.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<Userlogin> GetUserByIdAsync(string userId)
        {
            using var connection = CreateConnection();
            var query = "EXEC dbo.sp_GetUserById @UserId";

            return await connection.QueryFirstOrDefaultAsync<Userlogin>(query, new { UserId = userId });
        }

        public async Task<Userlogin?> AuthenticateUserAsync(string userId, string password)
        {
            using var connection = CreateConnection();
            var query = "EXEC dbo.sp_AuthenticateUser @UserId";
            var user = await connection.QueryFirstOrDefaultAsync<Userlogin>(query, new { UserId = userId });

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                _logger.LogWarning($"Invalid login attempt for User ID: {userId}");
                return null;
            }

            return user;
        }

        public async Task<int> InsertUserAsync(Userlogin user)
        {
            using var connection = CreateConnection();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password); // Hash password

            var query = @"EXEC dbo.sp_InsertUserlogin 
                          @UserId, @Password, @FullName, @DesignationId, 
                          @ProfilePicture, @CreatedBy, @IsLocked, @IsLoggedIn";

            try
            {
                return await connection.QuerySingleAsync<int>(query, user);
            }
            catch (SqlException ex)
            {
                _logger.LogError($"SQL Error: {ex.Message}");
                return 0; // Return failure
            }
        }
        // GetUsersAsync
        public async Task<IEnumerable<Userlogin>> GetUsersAsync()
        {
            using var connection = CreateConnection();
            var query = "EXEC dbo.sp_GetUsers";

            return await connection.QueryAsync<Userlogin>(query);
        }
    }
}
