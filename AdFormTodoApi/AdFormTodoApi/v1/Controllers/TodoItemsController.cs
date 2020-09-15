using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Services;
using AdFormTodoApi.v1.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AdFormTodoApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;
        private readonly IMapper _mapper;
        public TodoItemsController(ITodoItemService todoItemService, IMapper mapper)
        {
            _todoItemService = todoItemService;
            _mapper = mapper;


        }

        // GET: api/TodoItems
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
           var todoItems= await _todoItemService.GetAllTodoItem();
           var todoItemsDTO = _mapper.Map<IEnumerable<TodoItem>, IEnumerable<TodoItemDTO>>(todoItems);
            if (todoItems == null)
            {
                return NotFound(new { message = "TodoItem does not exists" });
            }
            return Ok(todoItemsDTO);
            
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _todoItemService.GetTodoItemById(id);
            var todoItemDTO = _mapper.Map<TodoItem,TodoItemDTO>(todoItem);

            if (todoItem == null)
            {
                return NotFound(new { message = "Todo Item does not exists" });
            }
            return Ok(todoItemDTO);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            var todoItem = _mapper.Map<TodoItemDTO, TodoItem>(todoItemDTO);
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            await _todoItemService.UpdateTodoItem(id,todoItem);
            return NoContent();
        }

        // POST: api/TodoItems
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(SaveTodoItemDTO todoItemDTO)
        {
            var todoItem = _mapper.Map<SaveTodoItemDTO, TodoItem>(todoItemDTO);
            if (todoItem.Description == null) 
                return BadRequest(new {message="TodoItem Description mandatory" });
            
            await _todoItemService.CreateTodoItem(todoItem);
            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            await _todoItemService.DeleteTodoItem(id);
            return NoContent();
        }

    }
}
