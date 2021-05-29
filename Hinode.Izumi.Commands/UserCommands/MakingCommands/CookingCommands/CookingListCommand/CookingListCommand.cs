using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService.Commands;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.EmoteService.Records;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CalculationService.Queries;
using Hinode.Izumi.Services.GameServices.FoodService.Queries;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using Hinode.Izumi.Services.GameServices.TutorialService.Commands;
using Hinode.Izumi.Services.ImageService.Queries;
using MediatR;
using Image = Hinode.Izumi.Data.Enums.Image;

namespace Hinode.Izumi.Commands.UserCommands.MakingCommands.CookingCommands.CookingListCommand
{
    [InjectableService]
    public class CookingListCommand : ICookingListCommand
    {
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;
        private Dictionary<string, EmoteRecord> _emotes;

        public CookingListCommand(IMediator mediator, ILocalizationService local)
        {
            _mediator = mediator;
            _local = local;
        }

        public async Task Execute(SocketCommandContext context, long masteryBracket)
        {
            // получаем иконки из базы
            _emotes = await _mediator.Send(new GetEmotesQuery());

            var embed = new EmbedBuilder()
                // баннер приготовления
                .WithImageUrl(await _mediator.Send(new GetImageUrlQuery(Image.Cooking)));

            // если пользователь не указал категорию - выводим список категорий
            if (masteryBracket == 0)
            {
                embed.WithDescription(IzumiReplyMessage.CookingListCategoryDesc.Parse())
                    .AddField(IzumiReplyMessage.CookingListCategoryFieldName.Parse(),
                        IzumiReplyMessage.CookingListCategoryFieldDesc.Parse(
                            _emotes.GetEmoteOrBlank("List"),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery0.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery50.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery100.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery150.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery200.Parse(),
                            IzumiReplyMessage.ShopRecipeFieldNameMastery250.Parse()));
            }
            else
            {
                // получаем рецепты пользователя
                var userRecipes = await _mediator.Send(new GetUserRecipesQuery((long) context.User.Id));

                // если у пользователя нет рецептов - рассказываем как их купить
                if (userRecipes.Length < 1)
                {
                    embed.WithDescription(IzumiReplyMessage.CookingListNull.Parse(
                        Location.Garden.Localize(true)));
                }
                else
                {
                    // рассказываем как приготовить блюдо
                    embed.WithDescription(
                        IzumiReplyMessage.CookingListDesc.Parse(
                            _emotes.GetEmoteOrBlank("Recipe")) +
                        $"\n{_emotes.GetEmoteOrBlank("Blank")}");

                    // определяем какое мастерство ввел пользователь
                    long foodMastery = 0;
                    switch (masteryBracket)
                    {
                        case 1:
                            foodMastery = 0;
                            break;
                        case 2:
                            foodMastery = 50;
                            break;
                        case 3:
                            foodMastery = 100;
                            break;
                        case 4:
                            foodMastery = 150;
                            break;
                        case 5:
                            foodMastery = 200;
                            break;
                        case 6:
                            foodMastery = 250;
                            break;
                        default:
                            await Task.FromException(new Exception(IzumiReplyMessage.CookingListCategoryWrong.Parse()));
                            break;
                    }

                    // для каждого рецепта создаем embed field
                    foreach (var food in userRecipes.Where(x => x.Mastery == foodMastery))
                    {
                        // получаем себестоимость блюда
                        var costPrice = await _mediator.Send(new GetFoodCostPriceQuery(food.Id));
                        // получаем стоимость приготовления
                        var cookingPrice = await _mediator.Send(new GetCraftingPriceQuery(costPrice));
                        // получаем количество восстанавливаемой энергии блюдом
                        var energy = await _mediator.Send(new GetFoodEnergyRechargeQuery(costPrice, cookingPrice));

                        embed.AddField(
                            // выводим иконку и название блюда
                            IzumiReplyMessage.CookingListFieldName.Parse(
                                _emotes.GetEmoteOrBlank("List"), food.Id, _emotes.GetEmoteOrBlank("Recipe"),
                                _local.Localize(food.Name, 2)),
                            // выводим информацию о блюде
                            IzumiReplyMessage.CookingListFieldDesc.Parse(
                                _emotes.GetEmoteOrBlank(food.Name), _local.Localize(food.Name),
                                _emotes.GetEmoteOrBlank("Energy"), energy, _local.Localize("Energy", energy)));
                    }
                }
            }

            await _mediator.Send(new SendEmbedToUserCommand(context.User, embed));

            // проверяем нужно ли двинуть прогресс обучения пользователя
            await _mediator.Send(new CheckUserTutorialStepCommand(
                (long) context.User.Id, TutorialStep.CheckCookingList));

            await Task.CompletedTask;
        }
    }
}
