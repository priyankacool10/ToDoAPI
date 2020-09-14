using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormTodoApi.Services
{
    public class AddParametersToSwagger : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && descriptor.ControllerName.StartsWith("Todo"))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "x-correlation-id",
                    In = ParameterLocation.Header,
                    Description = "Corrrelation ID",
                    Required = true
                });
                

            }
        }
    }
}
