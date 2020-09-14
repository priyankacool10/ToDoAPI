using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormTodoApi.Models
{
    public class TodoList:Todo
    {
        public List<TodoItem> TodoListOfItems { get; set; }
    }
}
