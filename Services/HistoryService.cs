using System.Collections;
using System.Text;
using Grpc.Core;
using GrpcHistory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GrpcHistory.Services 
{
    public class HistoryService: HistorySvc.HistorySvcBase
    {

        private readonly ConnectionFactory _cfRabbitMq;
        private  IConnection _rabbitMqConnection;
        
        private IModel channel;

        private readonly string _queueName = "history-winners-queue";

        private readonly ILogger<HistoryService> _logger;

        public HistoryService(ILogger<HistoryService> logger)
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

        public override async Task HistoryWinStream(Empty _, IServerStreamWriter<HistoryWinEventResponseStream> responseStream, ServerCallContext context)
        {

            Queue myQ = new Queue();

            var consumer = new EventingBasicConsumer(channel);
            
            
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"Received message : {message}");

                var data = new List<HistoryEvent> {
                    new HistoryEvent {
                        WinnerNickname = "sylvain",
                        ItemPrice = "10",
                        WinAt = 1,
                        ItemUrl = "http://localhost:3000"
                    }
                };

                var res = new HistoryWinEventResponseStream();
                res.HistoryEventList.AddRange(data);
                myQ.Enqueue(res);
            };


            while ( !context.CancellationToken.IsCancellationRequested ) {
                if ( myQ.Count > 0 ) {
                    var res = (HistoryWinEventResponseStream) myQ.Dequeue();
                    await responseStream.WriteAsync(res);
                }

            }


            // while ( !context.CancellationToken.IsCancellationRequested ) {
            //     //https://github.com/cactuaroid/GrpcWpfSample/blob/cd0d87ed6ccccac787960e9742aee036a3dc11eb/GrpcWpfSample.Server/Grpc/ChatServiceGrpcServer.cs#L51
            //     // https://stackoverflow.com/questions/60116274/grpc-c-sharp-server-side-waiting-until-client-closes-connection
            //     try {
            //         var data = new List<HistoryEvent> {
            //             new HistoryEvent {
            //                 WinnerNickname = "sylvain",
            //                 ItemPrice = "10",
            //                 WinAt = 1,
            //                 ItemUrl = "http://localhost:3000"
            //             },
            //             new HistoryEvent {
            //                 WinnerNickname = "sylvain2",
            //                 ItemPrice = "10",
            //                 WinAt = 1,
            //                 ItemUrl = "http://localhost:3000"
            //             }
            //         };

            //         var res = new HistoryWinEventResponseStream();
            //         res.HistoryEventList.AddRange(data);

            //         await responseStream.WriteAsync(res);

            //         //return;
            //     }  
                
            //         catch (Exception exception)
                
            //     {
            //         _logger.LogError(exception, "Error occurred");
            //         throw;
            //     }
                
            // } 



        }
    }
}