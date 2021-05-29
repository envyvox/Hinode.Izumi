using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Attributes
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
            if (services.GetService<IWebHostEnvironment>().IsDevelopment())
                return PreconditionResult.FromSuccess();

            var worldDebuff = (BossDebuff) await services.GetService<IMediator>()
                .Send(new GetPropertyValueQuery(Property.BossDebuff));

            return await Task.FromResult(worldDebuff == _bossDebuff
                ? PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireNoDebuff.Parse(
                    worldDebuff.Localize().ToLower()))
                : PreconditionResult.FromSuccess());
        }
    }
}
