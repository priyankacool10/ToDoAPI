using AdFormTodoApi.Models;
using System.Collections.Generic;

namespace AdFormTodoApi.Services
{
    public interface ITodoItemService
    {
        public IEnumerable<TodoItem> GetAllTodoItems();
    }
}
