using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlClient _sqlClient;
        private const string TableNameUser = "user";
        private const string TableNameKey = "apikey";

        public UserRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<UserRead> GetNameAsync(string username)
        {
            var sql = $"SELECT * FROM {TableNameUser} WHERE Username = @Username";

            return _sqlClient.QueryFirstOrDefaultAsync<UserRead>(sql, new { Username = username });
        }

        public Task<UserRead> GetNamePasswordAsync(string username, string password)
        {
            var sql = $"SELECT * FROM {TableNameUser} WHERE Username = @Username AND Password = @Password";

            return _sqlClient.QueryFirstOrDefaultAsync<UserRead>(sql, new { Username = username, Password = password });
        }

        public Task<int> SaveUserAsync(UserWrite user)
        {
            var sql = $"INSERT INTO {TableNameUser} (Id, Username, Password, DateCreated) VALUES(@Id, @Username, @Password, @DateCreated)";

            return _sqlClient.ExecuteAsync(sql, user);
        }

        public Task<IEnumerable<ApiKeyRead>> GetUserApiKeysAsync(Guid id)
        {
            var sql = $"SELECT * FROM {TableNameKey} WHERE Id = @Id";

            return _sqlClient.QueryAsync<ApiKeyRead>(sql, new { Id = id });
        }

        public Task<ApiKeyRead> GetApiKeyAsync(string key)
        {
            var sql = $"SELECT * FROM {TableNameKey} WHERE TokenKey = @TokenKey";

            return _sqlClient.QueryFirstOrDefaultAsync<ApiKeyRead>(sql, new { TokenKey = key });
        }

        public Task<int> SaveApiKeyAsync(ApiKeyWrite key)
        {
            var sql = $"INSERT INTO {TableNameKey} (Id, TokenKey, UserId, IsActive, DateCreated) VALUES(@Id, @TokenKey, @UserId, @IsActive, @DateCreated)";

            return _sqlClient.ExecuteAsync(sql, key);
        }
    }
}
