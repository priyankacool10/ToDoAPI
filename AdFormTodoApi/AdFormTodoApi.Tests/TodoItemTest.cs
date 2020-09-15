using AdFormTodoApi.Core;
using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Repositories;
using AdFormTodoApi.Core.Services;
using AdFormTodoApi.Service;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace AdFormTodoApi.Tests
{
    public class Tests
    {
        private Mock<IRepository<TodoItem>> _mockTodoItemRepository;
        private List<TodoItem> _todoItemList;
        private ITodoItemService _service;
        Mock<IUnitOfWork> _mockUnitWork;
        
        [SetUp]
        public void Setup()
        {
            _mockTodoItemRepository = new Mock<IRepository<TodoItem>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _service = new TodoItemService(_mockUnitWork.Object);
            _todoItemList = new List<TodoItem>() {
                       new TodoItem() { Id = 1, Description = "US Task", LabelId =1, TodoListId = 1 },
                       new TodoItem() { Id = 2, Description = "India Task", LabelId =1, TodoListId = 1 },
                       new TodoItem() { Id = 3, Description = "Russia Task", LabelId =1, TodoListId = 1 }
                      };
        }

        [Test]
        public void Test_GetAllTodoItems()
        {
            //Arrange
             _mockTodoItemRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(_todoItemList);

            //Act

            var list = _service.GetAllTodoItem();
            //Assert
            
        }
    }
}