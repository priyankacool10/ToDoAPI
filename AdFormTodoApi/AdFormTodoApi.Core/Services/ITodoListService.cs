using AdFormTodoApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormTodoApi.Core.Services
{
    public interface ITodoListService
    {
        Task<IEnumerable<TodoList>> GetAllTodoList();
        Task<TodoList> GetTodoListById(long id);
        Task<TodoList> CreateTodoList(TodoList newTodoList);
        Task UpdateTodoList(long todoItemId, TodoList todoListToBeUpdated);
        Task DeleteTodoList(long id);
    }
}
