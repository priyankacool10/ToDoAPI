namespace AdFormTodoApi.Models
{
    public class TodoItem :Todo
    {
        
        public string Description { get; set; }
        public bool isDone { get; set; } = false;
    }
}
