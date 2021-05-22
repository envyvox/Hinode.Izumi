using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.AchievementEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.AchievementService.Commands;
using Hinode.Izumi.Services.GameServices.AchievementService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.AchievementService.Handlers
{
    public class AddAchievementRewardToUserHandler : IRequestHandler<AddAchievementRewardToUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public AddAchievementRewardToUserHandler(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(AddAchievementRewardToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, achievementId) = request;
            var achievement = await _mediator.Send(new GetAchievementByIdQuery(achievementId), cancellationToken);
            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);

            switch (achievement.Reward)
            {
                case AchievementReward.Ien:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                            userId, InventoryCategory.Currency, Currency.Ien.GetHashCode(), achievement.Number),
                        cancellationToken);

                    break;
                case AchievementReward.Title:

                    await _mediator.Send(new AddTitleToUserCommand(userId, (Title) achievement.Number),
                        cancellationToken);

                    break;
                case AchievementReward.Pearl:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                            userId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), achievement.Number),
                        cancellationToken);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var embed = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.AchievementAdded.Parse(
                    emotes.GetEmoteOrBlank("Achievement"), achievement.Type.Localize(), achievement.Category.Localize(),
                    achievement.Reward switch
                    {
                        AchievementReward.Ien =>
                            $"{emotes.GetEmoteOrBlank(Currency.Ien.ToString())} {achievement.Number} {_local.Localize(Currency.Ien.ToString(), achievement.Number)}",

                        AchievementReward.Title =>
                            $"титул {emotes.GetEmoteOrBlank(((Title) achievement.Number).Emote())} {((Title) achievement.Number).Localize()}",

                        AchievementReward.Pearl =>
                            $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} {achievement.Number} {_local.Localize(Currency.Pearl.ToString(), achievement.Number)}",

                        _ => throw new ArgumentOutOfRangeException()
                    }));

            await _mediator.Send(new SendEmbedToUserCommand(
                    await _mediator.Send(new GetDiscordSocketUserQuery(userId), cancellationToken), embed),
                cancellationToken);

            return new Unit();
        }
    }
}
