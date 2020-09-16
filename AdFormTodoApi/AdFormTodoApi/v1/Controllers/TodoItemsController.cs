﻿using AdFormTodoApi.Core.Models;
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

        /// <summary>
        /// Method to get List of All TodoItems
        /// </summary>
        /// <param></param>
        /// <returns>List of TodoItems</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems([FromQuery]PagingOptions options)
        {
           var todoItems= await _todoItemService.GetAllTodoItem(options);
           var todoItemsDTO = _mapper.Map<IEnumerable<TodoItem>, IEnumerable<TodoItemDTO>>(todoItems);
            if (todoItems == null)
            {
                return NotFound(new { message = "TodoItem does not exists" });
            }
            return Ok(todoItemsDTO);
            
        } //paging and search

        /// <summary>
        /// Method to get TodoItem based on given ID
        /// </summary>
        /// <param name="id">Id of TodoItem</param>
        /// <returns>TodoItem</returns>
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

        /// <summary>
        /// Method to Update TodoItem based on given ID
        /// </summary>
        /// <param name="id,todoItemDTO"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Method to create a TodoItem
        /// </summary>
        /// <param name="todoItemDTO"></param>
        /// <returns>TodoItem</returns>
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

        /// <summary>
        /// Method to delete TodoItem of given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            await _todoItemService.DeleteTodoItem(id);
            return NoContent();
        }

    }
}
