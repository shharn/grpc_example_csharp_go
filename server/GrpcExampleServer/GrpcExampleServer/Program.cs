using Grpc.Core;
using GrpcExampleServer.Repository;
using GrpcExampleServer.Services;
using System;

namespace GrpcExampleServer
{
    class Program
    {
        const int Port = 50001;

        static void Main(string[] args)
        {
            Server server = new Server
            {
                Services = { Grpc.GameService.BindService(new GameGrpcServiceImpl(new GameRepository())) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine($"GameService server listening on port {Port}");
            Console.WriteLine($"Press any key to stop the server...");
            Console.ReadKey();

            server.ShutdownAsync().Wait();
        }
    }
}
