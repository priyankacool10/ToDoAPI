﻿
using System;

namespace AdFormTodoApi.Core.Models
{
    public class Todo
    {
       
   
        public DateTime CreatedDate { get; set; }
     
        public User CreatedBy { get; set; }
      
        public DateTime UpdatedDate { get; set; }
   
        public User UpdatedBy { get; set; }


    }
}
