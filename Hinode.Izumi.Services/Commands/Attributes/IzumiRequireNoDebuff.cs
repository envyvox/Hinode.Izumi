using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hinode.Izumi.Services.Commands.Attributes
{
    public class IzumiRequireNoDebuff : PreconditionAttribute
    {
        private readonly BossDebuff _bossDebuff;

        public IzumiRequireNoDebuff(BossDebuff bossDebuff)
        {
            _bossDebuff = bossDebuff;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            // в режиме разработки мы пропускаем проверку последствий вторжения босса
            if (services.GetService<IWebHostEnvironment>().IsDevelopment()) return PreconditionResult.FromSuccess();

            var propertyService = services.GetService<IPropertyService>();
            // получаем текущие последствия вторжения босса
            var worldDebuff = (BossDebuff) await propertyService.GetPropertyValue(Property.BossDebuff);

            // возвращаем результат в зависимости от проверки
            return await Task.FromResult(worldDebuff == _bossDebuff
                ? PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireNoDebuff.Parse(
                    worldDebuff.Localize().ToLower()))
                : PreconditionResult.FromSuccess());
        }
    }
}
