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
        private readonly ILogger<HistoryService> _logger;

        public HistoryService(ILogger<HistoryService> logger)
        {
            _logger = logger;
        }

        public override async Task HistoryWinStream(Empty _, IServerStreamWriter<HistoryWinEventResponseStream> responseStream, ServerCallContext context)
        {

            // TODO 
            // shared queue
            _logger.LogInformation("Call");

            Queue myQ = new Queue();
            var data = new List<HistoryEvent> {
                    new HistoryEvent {
                        WinnerNickname = "sylvain",
                        ItemPrice = "10",
                        WinAt = 1,
                        ItemUrl = "http://localhost:3000"
                    }
                };
            myQ.Enqueue(data);

            
            // _logger.LogInformation("Start listening to queue");
            // consumer.Received += async (model, ea) =>
            // {
            //     _logger.LogInformation("rrrrrr");
            //     var body = ea.Body.ToArray();
            //     var message = Encoding.UTF8.GetString(body);
            //     _logger.LogInformation($"Received message : {message}");
                
            //     await Task.Delay(50);

            //     var data = new List<HistoryEvent> {
            //         new HistoryEvent {
            //             WinnerNickname = "sylvain",
            //             ItemPrice = "10",
            //             WinAt = 1,
            //             ItemUrl = "http://localhost:3000"
            //         }
            //     };

            //     var res = new HistoryWinEventResponseStream();
            //     res.HistoryEventList.AddRange(data);
            //     myQ.Enqueue(res);
            // };

            _logger.LogInformation("Start consuming");
            while ( !context.CancellationToken.IsCancellationRequested ) {
                if ( myQ.Count > 0 ) {
                    var el = (List<HistoryEvent>) myQ.Dequeue();
                    var res = new HistoryWinEventResponseStream();
                    res.HistoryEventList.AddRange(el);
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