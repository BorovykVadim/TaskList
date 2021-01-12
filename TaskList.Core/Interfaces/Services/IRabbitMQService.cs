using System.Threading;
using System.Threading.Tasks;

namespace TaskList.Core.Interfaces.Services
{
    public interface IRabbitMQService
    {
        Task<string> Send(string message, CancellationToken cancellationToken = default);

        void Receive();
    }
}
