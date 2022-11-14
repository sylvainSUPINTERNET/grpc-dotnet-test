using System.Collections;
using System.Text;
using Grpc.Core;
using GrpcHistory;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Singleton;

namespace GrpcHistory.Services 
{
    public class HistoryService: HistorySvc.HistorySvcBase
    {
        private readonly ILogger<HistoryService> _logger;

        private readonly ISharedQueue _shareQueue;

        public HistoryService(ILogger<HistoryService> logger, ISharedQueue shareQueue)
        {
            _logger = logger;
            _shareQueue = shareQueue;
        }

        public override async Task HistoryWinStream(Empty _, IServerStreamWriter<HistoryWinEventResponseStream> responseStream, ServerCallContext context)
        {

            var id = Guid.NewGuid();
            _shareQueue.AddQueue(id);

            while ( !context.CancellationToken.IsCancellationRequested ) {
                if ( _shareQueue.GetQueue(id).Count > 0 ) {
                    Console.WriteLine($"Notify GUID stream : {id}");
                    var el = (HistoryEvent) _shareQueue.GetQueue(id).Dequeue();
                    var res = new HistoryWinEventResponseStream();
                    res.HistoryEventList.Add(el);
                    _shareQueue.GetQueue(id).Clear();
                    await responseStream.WriteAsync(res);
                }
            }

            Console.WriteLine("Cancel");

        }
    }
}