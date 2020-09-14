using Newtonsoft.Json;
using System;

namespace AdFormTodoApi.Models
{
    public abstract class Todo
    {
        public long Id { get; set; }
        [JsonIgnore]
        internal DateTime CreatedDate { get; set; }
        [JsonIgnore]
        internal DateTime UpdatedDate { get; set; } 
    }
}
