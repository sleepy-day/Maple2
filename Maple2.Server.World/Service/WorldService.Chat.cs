﻿using System.Threading.Tasks;
using Grpc.Core;

namespace Maple2.Server.World.Service;

public partial class WorldService {
    public override Task<ChatResponse> Chat(ChatRequest request, ServerCallContext context) {
        switch (request.ChatCase) {
            case ChatRequest.ChatOneofCase.Whisper: {
                WhisperChat(request);
                return Task.FromResult(new ChatResponse());
            }
            case ChatRequest.ChatOneofCase.Party:
                return Task.FromResult(new ChatResponse());
            case ChatRequest.ChatOneofCase.Guild:
                return Task.FromResult(new ChatResponse());
            case ChatRequest.ChatOneofCase.World:
                return Task.FromResult(new ChatResponse());
            case ChatRequest.ChatOneofCase.Super:
                return Task.FromResult(new ChatResponse());
            case ChatRequest.ChatOneofCase.Club:
                return Task.FromResult(new ChatResponse());
            case ChatRequest.ChatOneofCase.Wedding:
                return Task.FromResult(new ChatResponse());
            default:
                throw new RpcException(
                    new Status(StatusCode.InvalidArgument, $"Invalid chat type: {request.ChatCase}"));
        }
    }

    private void WhisperChat(ChatRequest request) {
        var channelChat = new Channel.Service.ChatRequest {
            AccountId = request.AccountId,
            CharacterId = request.CharacterId,
            Name = request.Name,
            Message = request.Message,
            Whisper = new Channel.Service.ChatRequest.Types.Whisper {
                RecipientId = request.Whisper.RecipientId,
                RecipientName = request.Whisper.RecipientName,
            }
        };

        // Ideally we would know which channel a character was on.
        foreach (Channel.Service.Channel.ChannelClient channel in channels) {
            try {
                // Once any request succeeds, we are done.
                channel.Chat(channelChat);
                return;
            } catch (RpcException) { }
        }

        throw new RpcException(new Status(StatusCode.NotFound, $"Unable to whisper: {request.Whisper.RecipientName}"));
    }
}
