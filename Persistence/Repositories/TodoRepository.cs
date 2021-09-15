using Contracts.Enums;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ISqlClient _sqlClient;
        private const string TableName = "todos";

        public TodoRepository(ISqlClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public Task<IEnumerable<TodoRead>> GetAllAsync(Guid userId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE UserId = @UserId";

            return _sqlClient.QueryAsync<TodoRead>(sql, new { UserId = userId });
        }

        public Task<TodoRead> GetAsync(Guid id, Guid userId)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id AND UserId = @UserId";

            return _sqlClient.QueryFirstOrDefaultAsync<TodoRead>(sql, new { Id = id, UserId = userId });
        }

        public Task<int> SaveOrUpdateAsync(TodoWrite todo)
        {
            var sql = $"INSERT INTO {TableName} (Id, Title, Description, Difficulty, DateCreated, IsDone, UserId) VALUES(@Id, @Title, @Description, @Difficulty, @DateCreated, @IsDone, @UserId)" +
                $"ON DUPLICATE KEY UPDATE Title = @Title, Description = @Description, Difficulty = @Difficulty, IsDone = @IsDone;";

            return _sqlClient.ExecuteAsync(sql, new {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Difficulty = todo.Difficulty.ToString(),
                DateCreated = todo.DateCreated,
                IsDone = todo.IsDone,
                UserId = todo.UserId
            });
        }

        public Task<int> DeleteAsync(Guid id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @Id;";

            return _sqlClient.ExecuteAsync(sql, new { Id = id });
        }
    }
}
