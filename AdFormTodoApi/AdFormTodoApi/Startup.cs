using AdFormTodoApi.Middleware;
using AdFormTodoApi.Models;
using AdFormTodoApi.Services;
using AdFormTodoApi.v1.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
namespace AdFormTodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           // services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
     
            // To Enable EF with SQL Server 
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoDatabase")));

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            }).AddNewtonsoftJson();
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // sepcify our operation filter here.  
                c.OperationFilter<AddParametersToSwagger>();

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = $"TODO v1 API",
                    Description = "TODO v1 API",
                    Contact = new OpenApiContact
                    {
                        Name = "Priyanka Kapoor",
                        Email = "priyanka.kapoor@nagarro.com",
                    }
                });
            });
            // Registering custom Authentication service
            services.AddScoped<IAuthenicateService, AuthenticateService>();
            services.AddScoped<CorrelationID, CorrelationID>();
            services.AddSession();
            services.AddDistributedMemoryCache();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My TODO API V1");
               
            });
            app.UseMiddleware<CorrelationIdToResponseMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ErrorLoggingMiddleware>();
            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
