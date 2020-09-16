using AdFormTodoApi.Core;
using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Repositories;
using AdFormTodoApi.Core.Services;
using AdFormTodoApi.Data;
using AdFormTodoApi.Service;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AdFormTodoApi.Tests
{
    public class Tests
    {
       
        private Mock<ITodoItemRepository> _mockTodoItemRepository;
        private List<TodoItem> _todoItemList;
        private TodoItem _todoItemModel;
        private ITodoItemService _service;
        private TodoContext _dbContext;
        IUnitOfWork _mockUnitWork;
        PagingOptions op;
        [SetUp]
        public void Setup()
        {
            var builder = new DbContextOptionsBuilder<TodoContext>()
             .UseInMemoryDatabase(databaseName: "MockTodoApiDatabase");
            _dbContext = new TodoContext(builder.Options);

            _mockTodoItemRepository = new Mock<ITodoItemRepository>();
            _mockUnitWork = new UnitOfWork(_dbContext);// { TodoItems = new TodoItemRepository(_dbcontext)  };
            _service = new TodoItemService(_mockUnitWork);
            op = new PagingOptions()
            {
                PageNumber = 1,
                PageSize = 3
            };
            _todoItemList = new List<TodoItem>() {
                       new TodoItem() { Id = 1, Description = "US Task", LabelId =1, TodoListId = 1 },
                       new TodoItem() { Id = 2, Description = "India Task", LabelId =1, TodoListId = 1 },
                       new TodoItem() { Id = 3, Description = "Russia Task", LabelId =1, TodoListId = 1 }
                      };
            _todoItemModel = new TodoItem() { Id = 1, Description = "US Task", LabelId = 1, TodoListId = 1 };
        }

        [Test]
        public void Test_GetAllTodoItems()
        {
            //Arrange
           // var repo = _mockTodoItemRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(_todoItemList);
           
            //Act
            var result = _mockUnitWork.TodoItems.GetAllTodoItemsAsync(op);
            
            //_mockTodoItemRepository.Verify();
            
        }

        
    }

            
}