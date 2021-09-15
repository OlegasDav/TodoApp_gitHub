using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface IUserRepository
    {
        Task<UserRead> GetNameAsync(string username);

        Task<UserRead> GetNamePasswordAsync(string username, string password);

        Task<int> SaveUserAsync(UserWrite user);

        Task<IEnumerable<ApiKeyRead>> GetUserApiKeysAsync(Guid id);

        Task<ApiKeyRead> GetApiKeyAsync(string key);

        Task<int> SaveApiKeyAsync(ApiKeyWrite key);
    }
}
