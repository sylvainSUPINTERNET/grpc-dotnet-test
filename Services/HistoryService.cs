using Grpc.Core;
using GrpcHistory;

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
            //https://github.com/cactuaroid/GrpcWpfSample/blob/cd0d87ed6ccccac787960e9742aee036a3dc11eb/GrpcWpfSample.Server/Grpc/ChatServiceGrpcServer.cs#L51
// https://stackoverflow.com/questions/60116274/grpc-c-sharp-server-side-waiting-until-client-closes-connection
            try {
                var data = new List<HistoryEvent> {
                    new HistoryEvent {
                        WinnerNickname = "sylvain",
                        ItemPrice = "10",
                        WinAt = 1,
                        ItemUrl = "http://localhost:3000"
                    },
                    new HistoryEvent {
                        WinnerNickname = "sylvain2",
                        ItemPrice = "10",
                        WinAt = 1,
                        ItemUrl = "http://localhost:3000"
                    }
                };

                var res = new HistoryWinEventResponseStream();
                res.HistoryEventList.AddRange(data);

                await responseStream.WriteAsync(res);

                //return;
            }  
            
                catch (Exception exception)
            
            {
                _logger.LogError(exception, "Error occurred");
                throw;
            }
            
        }
    }
}