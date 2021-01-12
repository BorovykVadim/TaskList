using Microsoft.Extensions.DependencyInjection;
using TaskList.Core.Interfaces.Services;
using TaskList.Core.Services;

namespace TaskList.SomeService
{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = new ServiceCollection()
                .AddScoped<IRabbitMQService, RabbitMQService>()
                .BuildServiceProvider();

            var service = serviceProvider.GetService<IRabbitMQService>();
            service.Receive();
        }
    }
}
