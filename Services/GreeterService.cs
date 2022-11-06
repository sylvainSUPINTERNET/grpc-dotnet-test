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
    

    public override Task<GreetResponse> Greet(GreetRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Greet method called");

        return Task.FromResult(new GreetResponse
        {
            Result = "Hello " + request.Greeting.FirstName
        });
    }

    public override async Task GreetStream(Empty _, IServerStreamWriter<GreetResponseStream> responseStream, ServerCallContext context)
    {
        _logger.LogInformation("GreetStreal method called");


        await responseStream.WriteAsync(new GreetResponseStream
        {
            Message = "Hello"
        });

        await Task.Delay(6000);

        await responseStream.WriteAsync(new GreetResponseStream
        {
            Message = "Hello2"
        });

        return;
    }


}
