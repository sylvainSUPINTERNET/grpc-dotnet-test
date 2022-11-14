using System.Runtime.InteropServices;
using BackgroundService;
using GrpcGreeter.Services;
using GrpcHistory.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Singleton;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection(); // Add this line for postman / testing (auto discovery grpc service)
builder.Services.AddSingleton<ISharedQueue, SharedQueue>();

builder.Services.AddHostedService<BackgroundtTasksService>();



if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) 
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Setup a HTTP/2 endpoint without TLS. ( macos )
        options.ListenLocalhost(50051, o => o.Protocols =
            HttpProtocols.Http2);
    });
}


var app = builder.Build();




IWebHostEnvironment env = app.Environment;
if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
    
}

// Configure the HTTP request pipeline.
// 50051 is the port where envoy redirect response from grpc cliet
app.MapGrpcService<GreeterService>();
app.MapGrpcService<HistoryService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
