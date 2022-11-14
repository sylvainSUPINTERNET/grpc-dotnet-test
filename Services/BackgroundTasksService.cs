using System.Collections;
using System.Text;
using Grpc.Core;
using GrpcHistory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace BackgroundService 
{
    class BackgroundtTasksService : IHostedService, IDisposable
    {
        private readonly ConnectionFactory _cfRabbitMq;
        private  IConnection _rabbitMqConnection;
        
        private IModel channel;

        private readonly string _queueName = "history-winners-queue";

        private readonly ILogger<BackgroundtTasksService> _logger;

        public BackgroundtTasksService(ILogger<BackgroundtTasksService> logger) 
        {
            _logger = logger;
            _cfRabbitMq = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest", Port = 5672 };
            _rabbitMqConnection = _cfRabbitMq.CreateConnection();
            channel = _rabbitMqConnection.CreateModel();
            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            _logger.LogInformation("Creating rabbitMQ connection for subscription");
            _logger.LogInformation($"Declare queue : {_queueName} (durable)");

        }

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting background service");

            var consumer = new EventingBasicConsumer(channel);
            
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                Console.WriteLine(body);
            };

            channel.CallbackException += (chann, args) =>
            {
                Console.WriteLine(args.Exception);
                _logger.LogInformation(args.Exception.Message);
            };

            channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);

            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken stoppingToken) 
        {
            _logger.LogInformation("Stopping background service, closing connection and channel");
            channel.Close();
            _rabbitMqConnection.Close();
            return Task.CompletedTask;
        }
    }
}
