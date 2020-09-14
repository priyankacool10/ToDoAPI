using AdFormTodoApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormTodoApi.v1.DataAccessLayer
{
    public class TodoItemRepository
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemRepository> _logger;
        public TodoItemRepository(TodoContext context, ILogger<TodoItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodoItems() 
        {
            _logger.LogInformation("Fetching list of all Todo Items");
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemByID(long id) 
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                _logger.LogError(DateTime.UtcNow + ": No TodoItem Exist with id : {0}", id);

            }
            else 
            {
                _logger.LogInformation(DateTime.UtcNow + ": Fetching Todo Item of ID: {0}", id);
            }
            
            return todoItem;

        }
        
    }
}

