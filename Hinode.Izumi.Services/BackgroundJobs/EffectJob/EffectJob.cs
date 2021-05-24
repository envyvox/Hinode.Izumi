using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.GameServices.EffectService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.EffectJob
{
    [InjectableService]
    public class EffectJob : IEffectJob
    {
        private readonly IMediator _mediator;

        public EffectJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task RemoveEffectFromUser(long userId, Effect effect) =>
            await _mediator.Send(new RemoveEffectFromUserCommand(userId, effect));
    }
}
