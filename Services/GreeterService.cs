using Grpc.Core;
using GrpcGreeter;
using Google.Protobuf.WellKnownTypes;

namespace GrpcGreeter.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override async Task SayHelloStream(Empty _, IServerStreamWriter<HelloReplyStream> responseStream, ServerCallContext context)
    {

        await responseStream.WriteAsync(new HelloReplyStream
        {
            Message = "Hello from stream1",
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        await Task.Delay(6000);

        await responseStream.WriteAsync(new HelloReplyStream
        {
            Message = "Hello from stream2",
            Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
        });

        return;
    }
}
