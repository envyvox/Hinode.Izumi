using System;
using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.GameServices.LocationService.Queries;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hinode.Izumi.Services.WebServices.CommandWebService.Attributes
{
    public class IzumiRequireLocation : PreconditionAttribute
    {
        public Location Location { get; }

        public IzumiRequireLocation(Location location) => Location = location;

        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            if (services.GetService<IWebHostEnvironment>().IsDevelopment())
                return PreconditionResult.FromSuccess();

            var userLocation = await services.GetService<IMediator>()
                .Send(new GetUserLocationQuery((long) context.User.Id));

            if (userLocation == Location) return PreconditionResult.FromSuccess();

            var errorMessage = userLocation switch
            {
                Location.InTransit => IzumiPreconditionErrorMessage.RequireLocationButYouInTransit.Parse(),
                Location.ExploreGarden => IzumiPreconditionErrorMessage.RequireLocationButYouExploringGarden.Parse(),
                Location.ExploreCastle => IzumiPreconditionErrorMessage.RequireLocationButYouExploringCastle.Parse(),
                Location.Fishing => IzumiPreconditionErrorMessage.RequireLocationButYouFishing.Parse(),
                Location.FieldWatering => IzumiPreconditionErrorMessage.RequireLocationButYouFieldWatering.Parse(),
                _ => IzumiPreconditionErrorMessage.RequireLocationButYouInAnother.Parse(Location.Localize())
            };

            return await Task.FromResult(PreconditionResult.FromError(errorMessage));
        }
    }
}
