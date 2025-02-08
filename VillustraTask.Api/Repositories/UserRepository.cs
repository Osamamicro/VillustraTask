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
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var query = @"EXEC dbo.sp_InsertUserlogin 
                          @UserId, @Password, @FullName, @DesignationId, 
                          @ProfilePicture, @CreatedBy, @IsLocked, @IsLoggedIn";
            return await connection.ExecuteAsync(query, user);
        }
    }
}
