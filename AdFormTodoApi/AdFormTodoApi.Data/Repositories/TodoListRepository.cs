using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormTodoApi.Data.Repositories
{
    class TodoListRepository : Repository<TodoList>, ITodoListRepository
    {
        public TodoListRepository(TodoContext context)
            : base(context)
        { }

        public async Task<IEnumerable<TodoList>> GetAllTodoListAsync(PagingOptions op)
        {
            return await TodoContext.TodoLists
                .Include(m => m.TodoItems)
                .Skip((op.PageNumber - 1) * op.PageSize)
                .Take(op.PageSize)
                .ToListAsync();
        }
                
        public async Task<TodoList> GetTodoListByIdAsync(long id)
        {
            return await TodoContext.TodoLists
                .Include(m => m.TodoItems)
                .SingleOrDefaultAsync(m => m.Id == id);
        }

        private TodoContext TodoContext
        {
            get { return Context as TodoContext; }
        }
    }
}