using Newtonsoft.Json;
using System;

namespace AdFormTodoApi.Models
{
    public class Todo
    {
       
        [JsonIgnore]
        internal DateTime CreatedDate { get; set; }
        [JsonIgnore]
        internal User CreatedBy { get; set; }
        [JsonIgnore]
        internal DateTime UpdatedDate { get; set; }
        [JsonIgnore]
        internal User UpdatedBy { get; set; }


    }
}
