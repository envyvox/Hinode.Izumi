using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.FamilyService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.UserService.Queries;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FamilyService.Queries
{
    public record GetFamilyEmbedViewQuery(EmbedBuilder EmbedBuilder, FamilyRecord Family) : IRequest<EmbedBuilder>;

    public class GetFamilyEmbedViewHandler : IRequestHandler<GetFamilyEmbedViewQuery, EmbedBuilder>
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public GetFamilyEmbedViewHandler(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task<EmbedBuilder> Handle(GetFamilyEmbedViewQuery request, CancellationToken cancellationToken)
        {
            var (embed, family) = request;
            _emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);

            embed
                // TODO добавить герб семьи
                .WithDescription(IzumiReplyMessage.FamilyInfoDesc.Parse(family.Name));

            if (family.Status == FamilyStatus.Registration)
            {
                embed.AddField(IzumiReplyMessage.FamilyInfoStatusRegistrationFieldName.Parse(),
                    IzumiReplyMessage.FamilyInfoStatusRegistrationFieldDesc.Parse());
            }
            else
            {
                var familyCurrency = await _mediator.Send(new GetFamilyCurrenciesQuery(family.Id), cancellationToken);
                var familyCurrencyString = Enum.GetValues(typeof(Currency))
                    .Cast<Currency>()
                    .Aggregate(string.Empty, (current, currency) =>
                        current +
                        $"{_emotes.GetEmoteOrBlank(currency.ToString())} {(familyCurrency.ContainsKey(currency) ? familyCurrency[currency].Amount : 0)} {_local.Localize(currency.ToString(), familyCurrency.ContainsKey(currency) ? familyCurrency[currency].Amount : 0)}, ");

                embed
                    .AddField(IzumiReplyMessage.FamilyInfoCurrencyFieldName.Parse(),
                        familyCurrencyString.Remove(familyCurrencyString.Length - 2))
                    .AddField(IzumiReplyMessage.FamilyInfoDescriptionFieldName.Parse(),
                        family.Description ?? IzumiReplyMessage.FamilyInfoDescriptionNull.Parse());
            }

            var familyUsers = await _mediator.Send(new GetFamilyUsersQuery(family.Id), cancellationToken);

            embed.AddField(IzumiReplyMessage.FamilyInfoMembersFieldName.Parse(),
                familyUsers.Aggregate(string.Empty,
                    (current, userFamilyModel) => current + DisplayFamilyUser(userFamilyModel).Result));

            return embed;
        }

        private async Task<string> DisplayFamilyUser(UserFamilyRecord userFamily)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(userFamily.UserId));
            return
                $"{_emotes.GetEmoteOrBlank("List")} {_emotes.GetEmoteOrBlank(user.Title.Emote())} {user.Title.Localize()} **{user.Name}**, {userFamily.Status.Localize()}\n";
        }
    }
}
