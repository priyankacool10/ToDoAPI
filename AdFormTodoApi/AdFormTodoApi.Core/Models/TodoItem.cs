namespace AdFormTodoApi.Core.Models
{
    public class TodoItem :Todo
    {
        public long Id { get; set; }
        public string Description { get; set; }
       
        public long LabelId { get; set; }
       
        public long TodoListId { get; set; }

        public TodoList TodoList { get; set; }



    }
}
