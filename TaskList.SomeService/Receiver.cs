using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using TaskList.Core.Interfaces.Services;

namespace TaskList.SomeService
{
    public class Receiver : IReceiver
    {
        private readonly ITaskService taskService;

        public Receiver(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        public void TakeMessage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "task_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.BasicQos(0, 1, false);
                    Console.WriteLine(" [*] Waiting for messages.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        int.TryParse(Encoding.UTF8.GetString(body), out int taskId);
                        Console.WriteLine(" [x] Received {0}", taskId);

                        UpdateTaskStatus(taskId);

                        Console.WriteLine(" [x] Done");

                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    channel.BasicConsume(queue: "task_queue",
                                         autoAck: false,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        private void UpdateTaskStatus(int taskId)
        {
            var rm = new Random();

            var time = rm.Next(1, 10);
            Console.WriteLine($"Time = {time}s");
            Thread.Sleep(time * 1000);

            this.taskService.UpdateTaskStatus(taskId, time);
        }
    }
}
