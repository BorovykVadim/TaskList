using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskList.Core.Interfaces.Repositories;
using TaskList.Core.Interfaces.Services;
using TaskList.Core.Services;
using TaskList.Data.Repositories;

namespace TaskList.Dependencies
{
    public static class DependenciesConfigurator
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository, Repository>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITaskService, TaskService>();
        }
    }
}
