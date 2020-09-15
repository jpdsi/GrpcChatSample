using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Grpc.Core;

using Chat;

namespace GrpcChatServer
{
    public class ChatService : Chat.ChatRoom.ChatRoomBase
    {
        private readonly ChatRoom _chatroomService;
        private readonly ILogger<ChatService> _logger;

        public ChatService(ChatRoom chatRoomService, ILogger<ChatService> logger)
        {
            _chatroomService = chatRoomService;
            _logger = logger;
        }

        public override async Task join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;

            do
            {
                if (!_chatroomService.HasJoined(requestStream.Current.User))
                {
                    _chatroomService.Join(requestStream.Current.User, responseStream);
                }
                await _chatroomService.BroadcastMessageAsync(requestStream.Current);
            } while (await requestStream.MoveNext());

            _chatroomService.Remove(context.Peer);

        }
    }
}
