using AdFormTodoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormTodoApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoItemsController> _logger;
        public TodoListsController(TodoContext context, ILogger<TodoItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/TodoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
        {
            _logger.LogInformation("Fetching list of all Todo Lists");
            return await _context.TodoLists.ToListAsync();
        }

        // GET: api/TodoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);

            if (todoList == null)
            {
                _logger.LogError(DateTime.UtcNow + ": No TodoItem Exist");
                return NotFound(new { message = "Todo Item does not exists" });
            }
            _logger.LogInformation(DateTime.UtcNow + ": Fetching Todo Items");
            return todoList;
        }

        // PUT: api/TodoLists/5
       [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoList(long id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                return BadRequest();
            }
            todoList.UpdatedDate = DateTime.UtcNow;
            _context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation(DateTime.UtcNow + ": Todo Item with {0} updated", id);
            return NoContent();
        }

        // POST: api/TodoLists
       [HttpPost]
        public async Task<ActionResult<TodoList>> PostTodoList(TodoList todoList)
        {
            todoList.CreatedDate = DateTime.UtcNow;
            _context.TodoLists.Add(todoList);
            await _context.SaveChangesAsync();
            _logger.LogInformation(DateTime.UtcNow + ": Todo List added");
            return CreatedAtAction("GetTodoList", new { id = todoList.Id }, todoList);
        }

        // DELETE: api/TodoLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoList>> DeleteTodoList(long id)
        {
            var todoList = await _context.TodoLists.FindAsync(id);
            if (todoList == null)
            {
                return NotFound(new { message = "Todo List does not exists" });
            }

            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();
            _logger.LogInformation(DateTime.UtcNow + ": TodoList with id: {0} deleted", id);
            return todoList;
        }

        private bool TodoListExists(long id)
        {
            return _context.TodoLists.Any(e => e.Id == id);
        }
    }
}
