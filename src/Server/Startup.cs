using System.Text.Json.Serialization;
using BxTestTask.Exceptions;
using BxTestTask.Handlers;
using BxTestTask.Stores;
using BxTestTask.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace BxTestTask
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
      services.AddControllers().AddJsonOptions(opts =>
      {
          opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
      });

      services.Configure<ApiBehaviorOptions>(opt =>
      {
          opt.SuppressModelStateInvalidFilter = true;
      });

      // In production, the React files will be served from this directory
      services.AddSpaStaticFiles(configuration =>
      {
        configuration.RootPath = "ClientApp/build";
      });

      services.AddSwaggerGen();

      services.Configure<TodoTaskStoreOption>(o => Configuration.GetSection("Store").Bind(o));

      services.AddSingleton<ITodoTaskStore>(services => 
        new ConcurrentStore(
          new TodoTaskStore(
            services.GetService<IOptions<TodoTaskStoreOption>>()
          )));
      services.AddSingleton<ICommandHandler<CreateTodoTaskCommand, CreateTodoTaskResult>, CreateTodoTaskCommandHandler>();
      services.AddSingleton<IQueryHandler<GetAllQuery, GetAllQueryResult>, GetAllQueryHandler>();
      services.AddSingleton<ICommandHandler<DeleteCommand>, DeleteCommandHandler>();
      services.AddSingleton<ICommandHandler<UpdateCommand>, UpdateCommandHandler>();

      services.AddSingleton<IValidator<NewTaskValidationContext>, NewTaskValidator>();
      services.AddSingleton<IValidator<UpdateTaskValidationContext>, UpdateTaskValidator>();
      services.AddSingleton<IValidator<DeleteTaskValidationContext>, DeleteTaskValidator>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ExceptionMiddleware>();
      app.UseHsts();

      app.UseStaticFiles();
      app.UseSpaStaticFiles();

      app.UseSwagger();

      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TodoListApi V1");
      });

      app.UseRouting();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller}/{action=Index}/{id?}");
      });

      app.UseSpa(spa =>
      {
        spa.Options.SourcePath = "ClientApp";

        if (env.IsDevelopment())
        {
          spa.UseReactDevelopmentServer(npmScript: "start");
        }
      });
    }
  }
}
