using BussinessLogic.Abstraction.Handlers;
using BussinessLogic.Abstraction.Model;
using BussinessLogic.Handlers.Implementation;
using BussinessLogic.Handlers.Validation;
using DataAccess.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLogic.Handlers.Extenstions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTodoListHandlers(this IServiceCollection services)
        {
            services.AddSingleton<ICreateTodoTaskValidator, CreateTodoTaskValidator>();
            services.AddSingleton<ICreateTodoTaskCommandHandler, CreateTodoTaskCommandHandler>();
 
            services.AddSingleton<IGetAllQueryHandler, GetAllQueryHandler>();

            services.AddSingleton<IDeleteTodoTaskValidator, DeleteTodoTaskValidator>();
            services.AddSingleton<IDeleteCommandHandler, DeleteCommandHandler>();

            services.AddSingleton<IUpdateTodoTaskValidator, UpdateTodoTaskValidator>();
            services.AddSingleton<IUpdateCommandHandler, UpdateCommandHandler>();

            return services;
        }
    }
}
