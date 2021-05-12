using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discord;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.DiscordServices.DiscordEmbedService;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;
using Hinode.Izumi.Services.EmoteService;
using Hinode.Izumi.Services.EmoteService.Impl;
using Hinode.Izumi.Services.EmoteService.Models;
using Hinode.Izumi.Services.RpgServices.BuildingService;
using Hinode.Izumi.Services.RpgServices.FamilyService.Models;
using Hinode.Izumi.Services.RpgServices.LocalizationService;
using Hinode.Izumi.Services.RpgServices.PropertyService;
using Hinode.Izumi.Services.RpgServices.UserService;
using Hinode.Izumi.Services.RpgServices.UserService.Models;

namespace Hinode.Izumi.Services.RpgServices.FamilyService.Impl
{
    [InjectableService]
    public class FamilyService : IFamilyService
    {
        private readonly IConnectionManager _con;
        private readonly IDiscordEmbedService _discordEmbedService;
        private readonly IDiscordGuildService _discordGuildService;
        private readonly IUserService _userService;
        private readonly IEmoteService _emoteService;
        private readonly ILocalizationService _local;
        private readonly IBuildingService _buildingService;
        private readonly IPropertyService _propertyService;

        private Dictionary<string, EmoteModel> _emotes;

        public FamilyService(IConnectionManager con, IDiscordEmbedService discordEmbedService,
            IDiscordGuildService discordGuildService, IUserService userService, IEmoteService emoteService,
            ILocalizationService local, IBuildingService buildingService, IPropertyService propertyService)
        {
            _con = con;
            _discordEmbedService = discordEmbedService;
            _discordGuildService = discordGuildService;
            _userService = userService;
            _emoteService = emoteService;
            _local = local;
            _buildingService = buildingService;
            _propertyService = propertyService;
        }

        public async Task<FamilyModel[]> GetAllFamilies() =>
            (await _con.GetConnection()
                .QueryAsync<FamilyModel>(@"
                    select * from families"))
            .ToArray();

        public async Task<FamilyModel> GetFamily(long id) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyModel>(@"
                    select * from families
                    where id = @id",
                    new {id});

        public async Task<FamilyModel> GetFamily(string name)
        {
            // получаем семью из базы
            var family = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyModel>(@"
                    select * from families
                    where name = @name",
                    new {name});

            // если семьи с таким названием нет - выводим ошибку
            if (family == null)
                await Task.FromException(new Exception(IzumiNullableMessage.FamilyByName.Parse()));

            // возвращаем семью
            return family;
        }

        public async Task<UserFamilyModel> GetUserFamily(long userId)
        {
            // получаем семью пользователя из базы
            var userFamily = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserFamilyModel>(@"
                    select * from user_families
                    where user_id = @userId",
                    new {userId});

            // если пользователь не состоит в семье - выводим ошибку
            if (userFamily == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserFamily.Parse()));

            // возвращаем семью пользователя
            return userFamily;
        }

        public async Task<UserModel> GetFamilyOwner(long familyId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserModel>(@"
                    select * from users
                    where id = (
                        select user_id from user_families
                        where family_id = @familyId
                          and status = @status)",
                    new {familyId, status = UserInFamilyStatus.Head});

        public async Task<bool> CheckUserHasFamily(long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_families
                    where user_id = @userId",
                    new {userId});

        public async Task<bool> CheckFamily(string name) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from families
                    where name = @name",
                    new {name});

        public async Task<FamilyInviteModel> GetFamilyInvite(long familyId, long userId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyInviteModel>(@"
                    select * from family_invites
                    where family_id = @familyId
                      and user_id = @userId",
                    new {familyId, userId});

        public async Task<FamilyInviteModel> GetFamilyInvite(long inviteId)
        {
            // получаем приглашение из базы
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyInviteModel>(@"
                    select * from family_invites
                    where id = @inviteId",
                    new {inviteId});

            // если такое приглашение есть - возвращаем его
            if (res != null) return res;

            // если нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.FamilyInvite.Parse()));
            return new FamilyInviteModel();
        }

        public async Task<FamilyInviteModel[]> GetFamilyInvites(long familyId) =>
            (await _con.GetConnection()
                .QueryAsync<FamilyInviteModel>(@"
                    select * from family_invites
                    where family_id = @familyId",
                    new {familyId}))
            .ToArray();

        public async Task<FamilyInviteModel[]> GetUserFamilyInvites(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<FamilyInviteModel>(@"
                    select * from family_invites
                    where user_id = @userId",
                    new {userId}))
            .ToArray();

        public async Task<FamilyCurrencyModel> GetFamilyCurrency(long familyId, Currency currency) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<FamilyCurrencyModel>(@"
                    select * from family_currencies
                    where family_id = @family_id
                      and currency = @currency",
                    new {familyId, currency});

        public async Task<Dictionary<Currency, FamilyCurrencyModel>> GetFamilyCurrency(long familyId) =>
            (await _con.GetConnection()
                .QueryAsync<FamilyCurrencyModel>(@"
                    select * from family_currencies
                    where family_id = @familyId",
                    new {familyId}))
            .ToDictionary(x => x.Currency);

        public async Task AddFamily(string name) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into families(status, name, description)
                    values (@status, @name, null)
                    on conflict (name) do nothing",
                    new {name, status = FamilyStatus.Registration});

        public async Task AddUserToFamily(long userId, long familyId)
        {
            // добавляем пользователя в семью
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_families(user_id, family_id, status)
                    values (@userId, @familyId, @status)
                    on conflict (user_id) do nothing",
                    new {userId, familyId, status = UserInFamilyStatus.Default});
            // проверяем нужно ли обновить статус семьи
            await CheckFamilyRegistrationComplete(familyId);
        }

        public async Task AddUserToFamily(long userId, string familyName)
        {
            // получаем семью
            var family = await GetFamily(familyName);
            // добавляем пользователя в нее
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_families(user_id, family_id, status)
                    values (@userId, @familyId, @status)
                    on conflict (user_id) do nothing",
                    new {userId, familyId = family.Id, status = UserInFamilyStatus.Head});
        }

        public async Task AddFamilyInvite(long familyId, long userId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into family_invites(family_id, user_id)
                    values (@familyId, @userId)
                    on conflict (family_id, user_id) do nothing",
                    new {familyId, userId});

        public async Task AddCurrencyToFamily(long familyId, Currency currency, long amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into family_currencies as fc (family_id, currency, amount)
                    values (@familyId, @currency, @amount)
                    on conflict (family_id, currency) do update
                        set amount = fc.amount + @amount,
                            updated_at = now()",
                    new {familyId, currency, amount});

        public async Task UpdateFamilyName(long familyId, string familyName) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update families
                    set name = @name,
                        updated_at = now()
                    where id = @familyId",
                    new {familyId, familyName});

        public async Task UpdateFamilyDescription(long familyId, string description) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update families
                    set description = @description,
                        updated_at = now()
                    where id = @familyId",
                    new {familyId, description});

        public async Task UpdateUserInFamilyStatus(long userId, UserInFamilyStatus status) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update user_families
                    set status = @status,
                        updated_at = now()
                    where user_id = @userId",
                    new {userId, status});

        public async Task RemoveFamily(long familyId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from families
                    where id = @familyId",
                    new {familyId});

        public async Task RemoveUserFromFamily(long userId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_families
                    where user_id = @userId",
                    new {userId});

        public async Task RemoveFamilyInvite(long inviteId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from family_invites
                    where id = @inviteId",
                    new {inviteId});

        public async Task RemoveCurrencyFromFamily(long familyId, Currency currency, long amount) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update family_currencies
                    set amount = amount - @amount,
                        updated_at = now()
                    where family_id = @familyId",
                    new {familyId, currency, amount});

        public async Task<EmbedBuilder> DisplayFamily(EmbedBuilder embed, FamilyModel family)
        {
            // получаем иконки из базы
            _emotes = await _emoteService.GetEmotes();

            embed
                // TODO добавить герб семьи
                .WithDescription(IzumiReplyMessage.FamilyInfoDesc.Parse(family.Name));

            // если семья не прошла этап регистрации - выводим информацию об этом
            if (family.Status == FamilyStatus.Registration)
            {
                embed.AddField(IzumiReplyMessage.FamilyInfoStatusRegistrationFieldName.Parse(),
                    IzumiReplyMessage.FamilyInfoStatusRegistrationFieldDesc.Parse());
            }
            // если прошла - нужно заполнить информацию о семье
            else
            {
                // получаем валюту семьи
                var familyCurrency = await GetFamilyCurrency(family.Id);
                // заполняем строку валюты семьи
                var familyCurrencyString = Enum
                    .GetValues(typeof(Currency))
                    .Cast<Currency>()
                    .Aggregate(string.Empty, (current, currency) =>
                        current +
                        $"{_emotes.GetEmoteOrBlank(currency.ToString())} {(familyCurrency.ContainsKey(currency) ? familyCurrency[currency].Amount : 0)} {_local.Localize(currency.ToString(), familyCurrency.ContainsKey(currency) ? familyCurrency[currency].Amount : 0)}, ");

                embed
                    // выводим валюту семьи
                    .AddField(IzumiReplyMessage.FamilyInfoCurrencyFieldName.Parse(),
                        familyCurrencyString.Remove(familyCurrencyString.Length - 2))
                    // выводим описание семьи
                    .AddField(IzumiReplyMessage.FamilyInfoDescriptionFieldName.Parse(),
                        family.Description ?? IzumiReplyMessage.FamilyInfoDescriptionNull.Parse());
            }

            // получаем членов семьи
            var familyUsers = await GetFamilyUsers(family.Id);

            embed.AddField(IzumiReplyMessage.FamilyInfoMembersFieldName.Parse(),
                // выводим информацию о членах семьи
                familyUsers.Aggregate(string.Empty,
                    (current, userFamilyModel) => current + DisplayFamilyUser(userFamilyModel).Result));

            // возвращаем embed-builder с информацией о семье
            return embed;
        }

        /// <summary>
        /// Возвращает локализированную строку пользователя.
        /// </summary>
        /// <param name="userFamilyModel">Семья пользователя.</param>
        /// <returns>Локализированное строка пользователя.</returns>
        private async Task<string> DisplayFamilyUser(UserFamilyModel userFamilyModel)
        {
            // получаем пользователя
            var user = await _userService.GetUser(userFamilyModel.UserId);
            // возвращаем локализированную строку пользователя
            return
                $"{_emotes.GetEmoteOrBlank("List")} {_emotes.GetEmoteOrBlank(user.Title.Emote())} {user.Title.Localize()} **{user.Name}**, {userFamilyModel.Status.Localize()}\n";
        }

        private async Task CheckFamilyRegistrationComplete(long familyId)
        {
            // получаем семью
            var family = await GetFamily(familyId);

            // если семья не находится на этапе регистрации - проверять нечего
            if (family.Status != FamilyStatus.Registration) return;

            // получаем пользователей семьи
            var familyUsers = await GetFamilyUsers(familyId);
            // получаем необходимое количество пользователей для завершения этапа регистрации
            var requiredUsersLength = await _propertyService.GetPropertyValue(Property.FamilyRegistrationUsers);

            // проверяем количество пользователей
            if (familyUsers.Length >= requiredUsersLength)
            {
                // обновляем статус семьи
                await UpdateFamilyStatus(familyId, FamilyStatus.Created);
                // добавляем семье постройку семейный дом
                await _buildingService.AddBuildingToFamily(familyId, Building.FamilyHouse);

                // получаем главу семьи
                var familyOwner = await GetFamilyOwner(familyId);
                var embed = new EmbedBuilder()
                    // уведомляем его о том, что семья прошла регистрацию
                    .WithDescription(IzumiReplyMessage.FamilyRegistrationCompleted.Parse(family.Name));

                await _discordEmbedService.SendEmbed(
                    await _discordGuildService.GetSocketUser(familyOwner.Id), embed);
            }
        }

        private async Task<UserFamilyModel[]> GetFamilyUsers(long familyId) =>
            (await _con.GetConnection()
                .QueryAsync<UserFamilyModel>(@"
                    select * from user_families
                    where family_id = @familyId
                    order by status desc",
                    new {familyId}))
            .ToArray();

        private async Task UpdateFamilyStatus(long familyId, FamilyStatus status) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    update families
                    set status = @status,
                        updated_at = now()
                    where id = @familyId",
                    new {familyId, status});
    }
}
