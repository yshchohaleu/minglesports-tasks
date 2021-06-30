using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minglesports.Tasks.Application;
using Minglesports.Tasks.BuildingBlocks;
using Minglesports.Tasks.BuildingBlocks.Mediatr;
using Minglesports.Tasks.BuildingBlocks.Messages;
using Minglesports.Tasks.BuildingBlocks.UserContext;
using Minglesports.Tasks.Core.Ports;
using Minglesports.Tasks.Providers.Entities;
using Minglesports.Tasks.Web.Services;

namespace Minglesports.Tasks.Web.IoC
{
    public static class Config
    {
        public static void ConfigureProviders(this IServiceCollection services, IConfiguration configuration)
        {
            // mediatr
            services.AddMediatR(typeof(IAppPointer).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

            // providers
            services.AddTransient<ITimeProvider, TimeProvider>();
            services.AddDbContext<TodoListDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TodoListDatabase"));
            });
            services.AddTransient<ISendMessages, MediatrMessageSender>();
            services.AddTransient<ITodoListUnitOfWork, TodoListUnitOfWork>();

            // user context
            services.AddScoped<IUserContextProvider, UserContextProvider>();
        }
    }
}