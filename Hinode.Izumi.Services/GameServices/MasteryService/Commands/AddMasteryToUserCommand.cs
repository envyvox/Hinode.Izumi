using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.MasteryService.Commands
{
    public record AddMasteryToUserCommand(long UserId, Mastery Mastery, double Amount) : IRequest;
}
