﻿using AdFormTodoApi.Core;
using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdFormTodoApi.Service
{
    public class TodoItemService : ITodoItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TodoItemService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<TodoItem> CreateTodoItem(TodoItem newTodoItem)
        {
            newTodoItem.CreatedDate = DateTime.UtcNow;
            await _unitOfWork.TodoItems.AddAsync(newTodoItem);
            await _unitOfWork.CommitAsync();
            return newTodoItem;
        }

        public async Task DeleteTodoItem(long id)
        {
            var todoItemToBeDeleted= await _unitOfWork.TodoItems
                .GetTodoItemByIdAsync(id);
            _unitOfWork.TodoItems.Remove(todoItemToBeDeleted);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodoItem()
        {
            return await _unitOfWork.TodoItems
                .GetAllTodoItemsAsync();
        }

        public async Task<TodoItem> GetTodoItemById(long id)
        {
            return await _unitOfWork.TodoItems
                .GetTodoItemByIdAsync(id);
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemByTodoListId(long todoListId)
        {
            return await _unitOfWork.TodoItems
                .GetTodoItemByTodoListIdAsync(todoListId);
        }

        public async Task UpdateTodoItem(long id, TodoItem newTodoItem)
        {
            var todoItemToBeUpdated = await _unitOfWork.TodoItems
                .GetTodoItemByIdAsync(id);
            todoItemToBeUpdated.Description = newTodoItem.Description;
            todoItemToBeUpdated.LabelId = newTodoItem.LabelId;
            todoItemToBeUpdated.UpdatedDate = DateTime.UtcNow;
            await _unitOfWork.CommitAsync();
        }
    }

}
