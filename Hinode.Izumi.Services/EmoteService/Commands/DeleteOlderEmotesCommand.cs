using System;
using MediatR;

namespace Hinode.Izumi.Services.EmoteService.Commands
{
    public record DeleteOlderEmotesCommand(DateTimeOffset DateTimeOffset) : IRequest;
}
