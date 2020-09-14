using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AdFormTodoApi.Models
{
    public class TodoItem :Todo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Description { get; set; }
       
        [ForeignKey("Label")]
        public long LabelId { get; set; }
        [JsonIgnore]
        public virtual Label Label { get; set; }

        [ForeignKey("TodoList")]
        public long TodoListId { get; set; }
        [JsonIgnore]
        public virtual TodoList TodoList { get; set; }


    }
}
