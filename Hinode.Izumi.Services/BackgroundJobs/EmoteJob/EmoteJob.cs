using System.Threading.Tasks;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.EmoteService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.BackgroundJobs.EmoteJob
{
    [InjectableService]
    public class EmoteJob : IEmoteJob
    {
        private readonly IMediator _mediator;

        public EmoteJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task UploadEmotes() => await _mediator.Send(new UploadEmotesCommand());
    }
}
