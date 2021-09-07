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
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoRead>> GetAllAsync();

        Task<TodoRead> GetAsync(Guid id);

        Task<int> SaveOrUpdateAsync(Guid id, TodoWrite todo);

        Task<int> DeleteAsync(Guid id);
    }
}
