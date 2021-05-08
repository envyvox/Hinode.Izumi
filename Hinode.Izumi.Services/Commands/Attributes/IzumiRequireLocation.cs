using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.LocationService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hinode.Izumi.Services.Commands.Attributes
{
    public class IzumiRequireLocation : PreconditionAttribute
    {
        private readonly Location _location;

        public IzumiRequireLocation(Location location)
        {
            _location = location;
        }

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            // в режиме разработки мы пропускаем проверку локации
            if (services.GetService<IWebHostEnvironment>().IsDevelopment()) return PreconditionResult.FromSuccess();

            var locationService = services.GetService<ILocationService>();
            // получаем текущую локацию пользователя
            var userLocation = await locationService.GetUserLocation((long) context.User.Id);

            // если текущая локация пользователя совпадает с необходимой - возвращаем успешную проверку
            if (userLocation == _location) return PreconditionResult.FromSuccess();

            // определяем текст ошибки в зависимости от локации
            var errorMessage = userLocation switch
            {
                Location.InTransit => IzumiPreconditionErrorMessage.RequireLocationButYouInTransit.Parse(),
                Location.ExploreGarden => IzumiPreconditionErrorMessage.RequireLocationButYouExploringGarden.Parse(),
                Location.ExploreCastle => IzumiPreconditionErrorMessage.RequireLocationButYouExploringCastle.Parse(),
                Location.Fishing => IzumiPreconditionErrorMessage.RequireLocationButYouFishing.Parse(),
                Location.FieldWatering => IzumiPreconditionErrorMessage.RequireLocationButYouFieldWatering.Parse(),
                _ => IzumiPreconditionErrorMessage.RequireLocationButYouInAnother.Parse(_location.Localize())
            };

            // возвращаем ошибку
            return await Task.FromResult(PreconditionResult.FromError(errorMessage));
        }
    }
}
