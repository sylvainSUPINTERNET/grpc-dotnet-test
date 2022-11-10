using Grpc.Core;
using GrpcHistory;

namespace GrpcHistory.Services 
{
    public class HistoryService: HistorySvc.HistorySvcBase
    {

        public override async Task HistoryWinStream(Empty _, IServerStreamWriter<HistoryWinEventResponseStream> responseStream, ServerCallContext context)
        {

            var eventsWinList = new HistoryEvent[5].Select(h => {
                var historyEvent = new HistoryEvent();
                historyEvent.WinnerNickname = $"sylvain {h}";
                historyEvent.ItemPrice = "10";
                historyEvent.WinAt = DateTime.Now.ToFileTime();
                historyEvent.ItemUrl = "http://localhost:3000";
                return historyEvent;
            }).ToArray();

            return Task.FromResult(new HistoryWinEventResponseStream
            {
                historyEventList = eventsWinList
            });
            
        }
    }
}