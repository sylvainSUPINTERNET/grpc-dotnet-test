using Grpc.Core;
using GrpcHistory;

namespace GrpcHistory.Services 
{
    public class HistoryService: HistorySvc.HistorySvcBase
    {


        public override async Task HistoryWinStream(Empty _, IServerStreamWriter<HistoryWinEventResponseStream> responseStream, ServerCallContext context)
        {

            // var eventsWinList = new HistoryEvent[5].Select(h => {
            //     var historyEvent = new HistoryEvent();
            //     historyEvent.WinnerNickname = $"sylvain {h}";
            //     historyEvent.ItemPrice = "10";
            //     historyEvent.WinAt = 1;
            //     historyEvent.ItemUrl = "http://localhost:3000";
            //     return historyEvent;
            // }).ToArray();

            // Console.WriteLine(eventsWinList.Length);
            // var rs = new Test();
            // rs.Msg = "ok";
            // await responseStream.WriteAsync(rs);
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

            return;
            
        }
    }
}