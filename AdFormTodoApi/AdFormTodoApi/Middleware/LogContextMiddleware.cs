using AdFormTodoApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormTodoApi.Middleware
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<LogContextMiddleware> logger;
        public LogContextMiddleware(RequestDelegate next, ILogger<LogContextMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public Task InvokeAsync(HttpContext context, CorrelationID correlationIDs)
        {
            context.Request.Headers.TryGetValue("x-correlation-id", out var traceValue);

            if (string.IsNullOrWhiteSpace(traceValue))
            {
                traceValue = new Guid().ToString();
                
            }
            context.Response.OnStarting((state) =>
            {
                context.Response.Headers.Add("x-correlation-id", traceValue);
                return Task.FromResult(0);
            }, context);

            ////await _next(context);
            //var correlationHeaders = context.Request.Headers
            //    .Where(h => h.Key.ToLowerInvariant().StartsWith("x-correlation-"))
            //    .ToDictionary(h => h.Key, h => (object)h.Value.ToString());

            //foreach (var correlationHeader in correlationHeaders)
            //{
            //    correlationIDs.Update(correlationHeader.Key, correlationHeader.Value.ToString());
            //}

           
            // ensures all entries are tagged with some values
            using (logger.BeginScope(correlationIDs.GetCurrentID()))
            {
                // Call the next delegate/middleware in the pipeline
                return next(context);
            }
        }
    }
}
