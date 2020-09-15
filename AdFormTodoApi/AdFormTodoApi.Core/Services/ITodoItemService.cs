using AdFormTodoApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdFormTodoApi.Core.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetAllTodoItem();
        Task<TodoItem> GetTodoItemById(long id);
        Task<IEnumerable<TodoItem>> GetTodoItemByTodoListId(long todoListId);
        Task<TodoItem> CreateTodoItem(TodoItem newTodoItem);
        Task UpdateTodoItem(long todoItemId, TodoItem todoItemToBeUpdated);
        Task DeleteTodoItem(long id);
    }
}
