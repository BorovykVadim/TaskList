namespace TaskList.Core.Interfaces.Services
{
    public interface IRabbitMQService
    {
        void Send(int time);

        void Receive();
    }
}
