using AdFormTodoApi.Core;
using AdFormTodoApi.Core.Helpers;
using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Services;
using AdFormTodoApi.Data;
using AdFormTodoApi.Helpers;
using AdFormTodoApi.Middleware;
using AdFormTodoApi.Service;
using AdFormTodoApi.v1.GraphiQL.Mutation;
using AdFormTodoApi.v1.GraphiQL.Queries;
using AdFormTodoApi.v1.GraphiQL.Types;
using AdFormTodoApi.v1.Middleware;
using AutoMapper;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

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

            services.AddCors();
            services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddType<TodoItemType>()
                .AddType<TodoListType>()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .Create());

            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // To Enable EF with SQL Server 
            services.AddDbContext<TodoContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TodoDatabase"), x => x.MigrationsAssembly("AdFormTodoApi.Data")));
            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
                
            }).AddNewtonsoftJson().AddXmlSerializerFormatters(); ;
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                
            }).AddXmlSerializerFormatters();

            //Authentication Registering

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
                //This adds a securityDefinition to the bottom of the Swagger document, which Swagger-UI renders as an “Authorize” button
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                /*
                 AddSecurityRequirement () method lets you define global security scheme which gets applied all the API 
                 whereas OperationFilter() allows us to add filter to only specific API based on attribute check like “Auhtorize’
                 */

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                //Scheme = "oauth2",
                                //Name = "Bearer",
                                //In = ParameterLocation.Header,
                            },
                            new string[] {}

                    }
                });

                // Swashbuckle.AspNetCore.Filters
                //The SecurityRequirementsOperationFilter adds a security property to each operation in the Swagger document, which renders in Swagger-UI as a padlock next to the operation
               // c.OperationFilter<SecurityRequirementsOperationFilter>();
                //c.OperationFilter<AuthOperationFilter>();
            });
            // Registering custom Authentication service
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<CorrelationID, CorrelationID>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITodoItemService, TodoItemService>();
            services.AddTransient<ITodoListService, TodoListService>();
            services.AddTransient<ILabelService, LabelService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = Configuration["AppSettings:Issuer"],
                    //ValidAudience = Configuration["AppSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSettings:Secret"]))
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UsePlayground();
            }
            app.UseCors();
            app.UseGraphQL("/graphql").UsePlayground("/graphql");
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My TODO API V1");
               
            });
            app.UseMiddleware<CorrelationIdToResponseMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<ErrorLoggingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();
            app.UseRouting();
            app.UseSession();

            app.UseAuthentication(); // this middleware should always be above Authorization.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
