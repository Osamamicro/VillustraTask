using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VillustraTask.Api.Interfaces;
using VillustraTask.Api.Models;

namespace VillustraTask.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
                return null;
            }
            return user;
        }

        public async Task<int> InsertUserAsync(Userlogin user)
        {
            using var connection = CreateConnection();

            // Hash the password before inserting
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var query = @"EXEC dbo.sp_InsertUserlogin 
                  @UserId, @Password, @FullName, @DesignationId, 
                  @ProfilePicture, @CreatedBy, @IsLocked, @IsLoggedIn";

            try
            {
                return await connection.QuerySingleAsync<int>(query, new
                {
                    user.UserId,
                    user.Password,
                    user.FullName,
                    user.DesignationId,
                    user.ProfilePicture,
                    user.CreatedBy,
                    user.IsLocked,
                    user.IsLoggedIn
                });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return 0; // Return failure
            }
        }
    }
}
