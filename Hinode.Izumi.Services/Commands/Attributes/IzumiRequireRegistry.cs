using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.UserService;
using Microsoft.Extensions.DependencyInjection;

namespace Hinode.Izumi.Services.Commands.Attributes
{
    public class IzumiRequireRegistry : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var userService = services.GetService<IUserService>();
            // проверяем есть ли пользователь в игровом мире
            var checkUser = await userService.CheckUser((long) context.User.Id);

            // возвращаем результат в зависимости от проверки
            return await Task.FromResult(checkUser
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError(IzumiPreconditionErrorMessage.RequireRegistry.Parse()));
        }
    }
}
