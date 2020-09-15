using AdFormTodoApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormTodoApi.Core.Repositories
{
    public interface ITodoListRepository : IRepository<TodoList>
    {
        Task<IEnumerable<TodoList>> GetAllTodoListAsync();
        Task<TodoList> GetTodoListByIdAsync(long id);
        
    }
}
