using Microsoft.EntityFrameworkCore;

namespace AdFormTodoApi.Models
{
    public class TodoContext:DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
