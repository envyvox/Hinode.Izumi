using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.EffectEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService.Queries;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.EffectService.Commands;
using Hinode.Izumi.Services.GameServices.EffectService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.PropertyService.Queries;
using Hinode.Izumi.Services.GameServices.StatisticService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.EffectService.Handlers
{
    public class AddEffectToUserHandler : IRequestHandler<AddEffectToUserCommand>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public AddEffectToUserHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<Unit> Handle(AddEffectToUserCommand request, CancellationToken cancellationToken)
        {
            var (userId, category, effect, expiration) = request;

            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_effects(user_id, category, effect, expiration)
                    values (@userId, @category, @effect, @expiration)
                    on conflict (user_id, effect) do nothing",
                    new {userId, category, effect, expiration});

            if (category == EffectCategory.Lottery) await CheckLottery();

            return new Unit();
        }

        private async Task CheckLottery()
        {
            var lotteryUsers = await _mediator.Send(new GetLotteryUsersCountQuery());
            var lotteryRequireUsers = await _mediator.Send(new GetPropertyValueQuery(Property.LotteryRequireUsers));

            if (lotteryUsers >= lotteryRequireUsers) await StartLottery();
        }

        private async Task StartLottery()
        {
            var emotes = await _mediator.Send(new GetEmotesQuery());
            var winner = await _mediator.Send(new GetRandomLotteryWinnerQuery());
            var lotteryAward = await _mediator.Send(new GetPropertyValueQuery(Property.LotteryAward));

            await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(
                winner.Id, InventoryCategory.Currency, Currency.Ien.GetHashCode(), lotteryAward));
            await _mediator.Send(new AddStatisticToUserCommand(winner.Id, Statistic.CasinoLotteryWin));

            var embedPm = new EmbedBuilder()
                .WithDescription(IzumiReplyMessage.LotteryWinnerPm.Parse(
                    emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    lotteryAward, _local.Localize(Currency.Ien.ToString(), lotteryAward)));

            await _mediator.Send(new SendEmbedToUserCommand(
                await _mediator.Send(new GetDiscordSocketUserQuery(winner.Id)), embedPm));

            var embedNotify = new EmbedBuilder()
                .WithAuthor(IzumiEventMessage.DiaryAuthorField.Parse())
                .WithDescription(IzumiEventMessage.LotteryWinner.Parse(
                    emotes.GetEmoteOrBlank(winner.Title.Emote()), winner.Title.Localize(), winner.Name,
                    emotes.GetEmoteOrBlank("LotteryTicket"), emotes.GetEmoteOrBlank(Currency.Ien.ToString()),
                    lotteryAward, _local.Localize(Currency.Ien.ToString(), lotteryAward)));

            await _mediator.Send(new SendEmbedToChannelCommand(DiscordChannel.Diary, embedNotify));
        }
    }
}
