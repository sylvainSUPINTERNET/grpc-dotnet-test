
# Start envoy for conversion http to http2 ( grpc requirement )

```` bash

docker-compose up — build -d

````

When the Envoy server is running, it will proxy every response from the port 8080 (the client app should send in here) to the 50051 (the server app should listen to this port) port.


```` bash 

# Don't start dotnet BEFORE envoy else you will facing port in usage issue

dotnet run 

````


# RABBITMQ

````bash 

docker run -d --hostname my-rabbitmq-server --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

````

# Payload test

```` bash 

{
 "winnerNickname": "sylva",
  "itemPrice": "2",
  "winAt": "12457896",
  "itemUrl": "http://localhost:3000"
}

````