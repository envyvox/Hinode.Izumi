using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.ReferralService.Commands;
using Hinode.Izumi.Services.GameServices.ReferralService.Queries;
using Hinode.Izumi.Services.GameServices.UserService.Commands;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ReferralService.Handlers
{
    public class CreateUserReferrerHandler : IRequestHandler<CreateUserReferrerCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public CreateUserReferrerHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(CreateUserReferrerCommand request, CancellationToken cancellationToken)
        {
            var (userId, referrerId) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_referrers(user_id, referrer_id)
                    values (@userId, @referrerId)",
                    new {userId, referrerId});

            await _mediator.Send(
                new AddItemToUserByInventoryCategoryCommand(userId, InventoryCategory.Box, Box.Capital.GetHashCode()),
                cancellationToken);
            await AddRewardsToReferrer(userId, referrerId);

            return new Unit();
        }

        private async Task AddRewardsToReferrer(long userId, long referrerId)
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var referralCount = await _mediator.Send(new GetUserReferralCountQuery(userId));

            var rewardString = string.Empty;
            switch (referralCount)
            {
                case 1 or 2:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        referrerId, InventoryCategory.Box, Box.Capital.GetHashCode()));

                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} {_local.Localize(Box.Capital.ToString())}";

                    break;
                case 3 or 4:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        referrerId, InventoryCategory.Box, Box.Capital.GetHashCode(), 2));

                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} 2 {_local.Localize(Box.Capital.ToString(), 2)}";

                    break;
                case 5:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        referrerId, InventoryCategory.Box, Box.Capital.GetHashCode(), 3));

                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Box.Capital.Emote())} 3 {_local.Localize(Box.Capital.ToString(), 3)}";

                    break;
                case 6 or 7 or 8 or 9:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        referrerId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), 10));

                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} 10 {_local.Localize(Currency.Pearl.ToString(), 10)}";

                    break;
                case 10:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        referrerId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), 10));
                    await _mediator.Send(new AddTitleToUserCommand(referrerId, Title.Yatagarasu));

                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} 10 {_local.Localize(Currency.Pearl.ToString(), 10)}, " +
                        $"титул {emotes.GetEmoteOrBlank(Title.Yatagarasu.Emote())} {Title.Yatagarasu.Localize()}";

                    break;
                case > 10:

                    await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                        referrerId, InventoryCategory.Currency, Currency.Pearl.GetHashCode(), 15));

                    rewardString =
                        $"{emotes.GetEmoteOrBlank(Currency.Pearl.ToString())} 15 {_local.Localize(Currency.Pearl.ToString(), 15)}";

                    break;
            }

            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            var embedPm = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.ReferralSetNotifyPm.Parse(
                    emotes.GetEmoteOrBlank(user.Title.Emote()), user.Title.Localize(), user.Name))
                .AddField(IzumiReplyMessage.ReferralRewardFieldName.Parse(), rewardString);

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(referrerId)), embedPm));
        }
    }
}
