using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Services;
using AdFormTodoApi.v1.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormTodoApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListService _todoListService;
        private readonly IMapper _mapper;
        public TodoListsController(ITodoListService todoListService, IMapper mapper)
        {
            _todoListService = todoListService;
            _mapper = mapper;
        }

        // GET: api/TodoLists
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TodoList>>> GetTodoLists()
        {
            var todoList = await _todoListService.GetAllTodoList();
            var todoListDTO = _mapper.Map<IEnumerable<TodoList>, IEnumerable<TodoListDTO>>(todoList);
            if (todoList == null)
            {
                return NotFound(new { message = "TodoList does not exists" });
            }
            return Ok(todoListDTO);
        }

        // GET: api/TodoLists/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            var todoList = await _todoListService.GetTodoListById(id);
            var todoListDTO = _mapper.Map<TodoList,TodoListDTO>(todoList);

            if (todoList == null)
            {
                return NotFound(new { message = "Todo List does not exists" });
            }
            return Ok(todoListDTO);
        }

        // PUT: api/TodoLists/5
       [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoList(long id, TodoListDTO todoListDTO)
        {
            var todoList = _mapper.Map<TodoListDTO, TodoList>(todoListDTO);
            if (id != todoList.Id)
            {
                return BadRequest();
            }

            await _todoListService.UpdateTodoList(id, todoList);
            return NoContent();
        }

        // POST: api/TodoLists
       [HttpPost]
        public async Task<ActionResult<TodoList>> PostTodoList(TodoListDTO todoListDTO)
        {
            var todoList = _mapper.Map<TodoListDTO,TodoList>(todoListDTO);
            if (todoList.Description == null)
                return BadRequest(new { message = "TodoList Description mandatory" });

            await _todoListService.CreateTodoList(todoList);
            return CreatedAtAction("GetTodoList", new { id = todoList.Id }, todoList);
        }

        // DELETE: api/TodoLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoList>> DeleteTodoList(long id)
        {
            await _todoListService.DeleteTodoList(id);
            return NoContent();
        }

    }
}
