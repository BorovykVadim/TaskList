using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using TaskList.Core.Interfaces.Repositories;
using TaskList.Core.Interfaces.Services;
using TaskList.Core.Services;
using TaskList.Data;
using TaskList.Data.Repositories;

namespace TaskList.SomeService
{
    class Program
    {
        private static IConfiguration _iconfiguration;

        static void Main(string[] args)
        {
            GetAppSettingsFile();

            var serviceProvider = new ServiceCollection()
                .AddDbContext<TaskListDbContext>(config =>
                {
                    config.UseSqlServer(_iconfiguration.GetConnectionString("DefaultConnection"));
                })
                .AddScoped<IRepository, Repository>()
                .AddScoped<ITaskService, TaskService>()
                .AddScoped<IReceiver, Receiver>()
                .BuildServiceProvider();

            var receiver = serviceProvider.GetService<IReceiver>();
            receiver.TakeMessage();
        }

        static void GetAppSettingsFile()
        {
            var builder = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                                 _iconfiguration = builder.Build();
        }
    }
}
