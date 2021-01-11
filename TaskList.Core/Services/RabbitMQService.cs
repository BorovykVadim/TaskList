using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using TaskList.Core.Interfaces.Services;

namespace TaskList.Core.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        public void Send(int time)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "task_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            var body = Encoding.UTF8.GetBytes(time.ToString());

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: "task_queue",
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine(" [x] Sent {0}", time);

            Console.WriteLine(" Press [enter] to exit.");
        }

        public void Receive()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

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
                int.TryParse(Encoding.UTF8.GetString(body), out int time);
                Console.WriteLine(" [x] Received {0}", time);

                Thread.Sleep(time * 1000);

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
