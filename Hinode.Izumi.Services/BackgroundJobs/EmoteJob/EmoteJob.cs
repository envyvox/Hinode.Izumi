using System.Threading.Tasks;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.EmoteService;

namespace Hinode.Izumi.Services.BackgroundJobs.EmoteJob
{
    [InjectableService]
    public class EmoteJob : IEmoteJob
    {
        private readonly IEmoteService _emoteService;

        public EmoteJob(IEmoteService emoteService)
        {
            _emoteService = emoteService;
        }

        public async Task UploadEmotes() => await _emoteService.UploadEmotes();
    }
}
