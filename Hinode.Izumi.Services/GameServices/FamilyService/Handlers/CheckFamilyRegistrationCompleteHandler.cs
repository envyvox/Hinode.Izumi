using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.GameServices.BuildingService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Commands;
using Hinode.Izumi.Services.GameServices.FamilyService.Queries;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Handlers
{
    public class CheckFamilyRegistrationCompleteHandler : IRequestHandler<CheckFamilyRegistrationCompleteCommand>
    {
        private readonly IMediator _mediator;

        public CheckFamilyRegistrationCompleteHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CheckFamilyRegistrationCompleteCommand request,
            CancellationToken cancellationToken)
        {
            var family = await _mediator.Send(new GetFamilyByIdQuery(request.FamilyId), cancellationToken);

            if (family.Status != FamilyStatus.Registration) return new Unit();

            var familyUsers = await _mediator.Send(
                new GetFamilyUsersQuery(family.Id), cancellationToken);
            var requiredUsersLength = await _mediator.Send(
                new GetPropertyValueQuery(Property.FamilyRegistrationUsers), cancellationToken);

            if (familyUsers.Length >= requiredUsersLength)
            {
                await _mediator.Send(
                    new UpdateFamilyStatusCommand(family.Id, FamilyStatus.Created), cancellationToken);
                await _mediator.Send(
                    new AddBuildingToFamilyCommand(family.Id, Building.FamilyHouse), cancellationToken);

                var familyOwner = await _mediator.Send(new GetFamilyOwnerQuery(family.Id), cancellationToken);
                var embed = new EmbedBuilder()
                    .WithDescription(IzumiReplyMessage.FamilyRegistrationCompleted.Parse(family.Name));

                await _mediator.Send(new SendEmbedToUserCommand(
                        await _mediator.Send(new GetDiscordSocketUserQuery(familyOwner.Id), cancellationToken), embed),
                    cancellationToken);
            }

            return new Unit();
        }
    }
}
