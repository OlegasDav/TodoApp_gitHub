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
        Task<IEnumerable<TodoRead>> GetAllAsync(Guid userId);

        Task<TodoRead> GetAsync(Guid id, Guid userId);

        Task<int> SaveOrUpdateAsync(TodoWrite todo);

        Task<int> DeleteAsync(Guid id);
    }
}
