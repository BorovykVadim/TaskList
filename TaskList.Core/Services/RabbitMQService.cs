using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskList.Core.Interfaces.Services;

namespace TaskList.Core.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private const string QueueName = "new_queue";

        private readonly IConnection _connection;
        private readonly IModel _channelSender;
        private static IModel _channelRec;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;

        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>>
            _callbackMapper = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

        public RabbitMQService()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            _connection = factory.CreateConnection();
            _channelSender = _connection.CreateModel();
            _replyQueueName = _channelSender.QueueDeclare().QueueName;

            _consumer = new EventingBasicConsumer(_channelSender);
            _consumer.Received += OnReceivedSender;
        }

        public Task<string> Send(string message, CancellationToken cancellationToken = default)
        {
            Console.WriteLine(" [x] Sending message");

            var correlationId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<string>();
            _callbackMapper.TryAdd(correlationId, tcs);

            var props = _channelSender.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = _replyQueueName;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channelSender.BasicPublish("", QueueName, props, messageBytes);
            _channelSender.BasicConsume(consumer: _consumer, queue: _replyQueueName, autoAck: true);

            cancellationToken.Register(() =>
                _callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
        }

        public void Receive()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            using var connection = factory.CreateConnection();
            _channelRec = connection.CreateModel();

            _channelRec.QueueDeclare(QueueName, false, false, false, null);
            _channelRec.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(_channelRec);
            consumer.Received += OnReceivedRec;

            _channelRec.BasicConsume(QueueName, false, consumer);

            Console.WriteLine(" [x] Awaiting for tasks");
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        private void OnReceivedSender(object model, BasicDeliverEventArgs ea)
        {
            var suchTaskExists = _callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs);

            if (!suchTaskExists) return;

            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);

            tcs.TrySetResult(response);

            Console.WriteLine(" [.] Got '{0}'", response);
        }

        private static async Task OnReceivedRec(object model, BasicDeliverEventArgs ea)
        {
            string response = null;

            var body = ea.Body.ToArray();
            var props = ea.BasicProperties;
            var replyProps = _channelRec.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            try
            {
                var message = Encoding.UTF8.GetString(body);
                var time = int.Parse(message);
                Thread.Sleep(time * 1000);
                Console.WriteLine(time);
                response = "Task complite";
            }
            catch (Exception e)
            {
                Console.WriteLine(" [.] " + e.Message);
                response = "";
            }
            finally
            {
                var responseBytes = Encoding.UTF8.GetBytes(response);

                _channelRec.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
                _channelRec.BasicAck(ea.DeliveryTag, false);
            }
        }
    }
}
