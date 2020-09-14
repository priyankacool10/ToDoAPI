using AdFormTodoApi.Models;
using AdFormTodoApi.v1.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormTodoApi.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly TodoItemRepository _todoItemRepo;
        public TodoItemService(TodoItemRepository todoItemRepo) {
            _todoItemRepo = todoItemRepo;


        }
        public async Task<IEnumerable<TodoItem>> GetAllTodoItems()
        {
            return await _todoItemRepo.GetAllTodoItems();
        }

        public async Task<TodoItem> GetTodoItemByID(long id) 
        {
            return await _todoItemRepo.GetTodoItemByID(id);


        }

        IEnumerable<TodoItem> ITodoItemService.GetAllTodoItems()
        {
            throw new NotImplementedException();
        }
    }
}
