using MediatR;

namespace Hinode.Izumi.Services.GameServices.MarketService.Commands
{
    public record DeleteMarketRequestCommand(long Id) : IRequest;
}
