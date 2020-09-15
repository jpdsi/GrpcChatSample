using Chat;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcChatServer
{
    public class ChatRoom
    {
        private ConcurrentDictionary<string, IServerStreamWriter<Message>> users = new ConcurrentDictionary<string, IServerStreamWriter<Message>>();

        public bool HasJoined(string name) => users.ContainsKey(name);
        public void Join(string name, IServerStreamWriter<Message> response)
        {
            users.TryAdd(name, response);
            Console.WriteLine($"[INFO] {name} has joined the rooom.");
        }
        public void Remove(string name)
        {
            users.TryRemove(name, out var _);
            Console.WriteLine($"[INFO] {name} has left the room.");
        }

        public async Task BroadcastMessageAsync(Message message)
        {
            await BroadcastMessages(message);
            Console.WriteLine($"[INFO] {message.User} has broadcasted a message '{message.Text}'.");
        }

        private async Task BroadcastMessages(Message message)
        {
            foreach (var user in users.Where(x => x.Key != message.User))
            {
                var item = await SendMessageToSubscriber(user, message);
                if (item != null)
                {
                    Remove(item?.Key);
                };
            }
        }

        private async Task<Nullable<KeyValuePair<string, IServerStreamWriter<Message>>>> SendMessageToSubscriber(KeyValuePair<string, IServerStreamWriter<Message>> user, Message message)
        {
            try
            {
                await user.Value.WriteAsync(message);
                // Console.WriteLine($"[INFO] broadcast message '{message.Text}' from '{message.User}'.");
                return null;
            }
            catch (Exception)
            {
                // Console.WriteLine(ex);
                return user;
            }
        }
    }
}
