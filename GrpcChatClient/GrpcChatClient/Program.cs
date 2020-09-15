using System;
using System.Threading;
using System.Threading.Tasks;

using Grpc.Net.Client;
using Chat;

namespace GrpcChatClient
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("[INFO] Hello World! I am GrpcChatClient!");
            Console.Write("Input your user name: ");

            var userName = Console.ReadLine();

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new ChatRoom.ChatRoomClient(channel);

            using (var chat = client.join())
            {
                _ = Task.Run(async () =>
                {
                    while (await chat.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                    {
                        var response = chat.ResponseStream.Current;
                        Console.WriteLine($"[{response.User}]: {response.Text}");
                    }
                });

                await chat.RequestStream.WriteAsync(new Message { User = userName, Text = $"[INFO] {userName} has joined the room." });

                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (line.ToLower() == "bye")
                    {
                        break;
                    }
                    await chat.RequestStream.WriteAsync(new Message { User = userName, Text = line });
                }
                await chat.RequestStream.CompleteAsync();
            }

            Console.WriteLine("[INFO] Disconnecting...");
            await channel.ShutdownAsync();
        }
    }
}
