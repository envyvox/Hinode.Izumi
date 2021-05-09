using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.RpgServices.EffectService;

namespace Hinode.Izumi.Services.BackgroundJobs.EffectJob
{
    [InjectableService]
    public class EffectJob : IEffectJob
    {
        private readonly IEffectService _effectService;

        public EffectJob(IEffectService effectService)
        {
            _effectService = effectService;
        }


        public async Task RemoveEffectFromUser(long userId, Effect effect) =>
            await _effectService.RemoveEffectFromUser(userId, effect);
    }
}
