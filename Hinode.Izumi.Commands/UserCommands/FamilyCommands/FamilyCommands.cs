using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.Attributes;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyCheckInfoCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyInfoCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyRegisterCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyAddCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyTakeCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyKickUserCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyUpdateUserStatusCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteAcceptCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteCancelCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteDeclineCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteListCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteSendCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyDeleteCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyRenameCommand;
using Hinode.Izumi.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyUpdateDescriptionCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.FamilyEnums;

namespace Hinode.Izumi.Commands.UserCommands.FamilyCommands
{
    [CommandCategory(CommandCategory.Family)]
    [Group("семья"), Alias("family")]
    [IzumiRequireRegistry]
    public class FamilyCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IFamilyInfoCommand _familyInfoCommand;
        private readonly IFamilyRegisterCommand _familyRegisterCommand;
        private readonly IFamilyInviteListCommand _familyInviteListCommand;
        private readonly IFamilyKickUserCommand _familyKickUserCommand;
        private readonly IFamilyUpdateUserStatusCommand _familyUpdateUserStatusCommand;
        private readonly IFamilyRenameCommand _familyRenameCommand;
        private readonly IFamilyDeleteCommand _familyDeleteCommand;
        private readonly IFamilyUpdateDescriptionCommand _familyUpdateDescriptionCommand;
        private readonly IFamilyCheckInfoCommand _familyCheckInfoCommand;

        public FamilyCommands(IFamilyInfoCommand familyInfoCommand, IFamilyRegisterCommand familyRegisterCommand,
            IFamilyInviteListCommand familyInviteListCommand, IFamilyKickUserCommand familyKickUserCommand,
            IFamilyUpdateUserStatusCommand familyUpdateUserStatusCommand, IFamilyRenameCommand familyRenameCommand,
            IFamilyDeleteCommand familyDeleteCommand, IFamilyUpdateDescriptionCommand familyUpdateDescriptionCommand,
            IFamilyCheckInfoCommand familyCheckInfoCommand)
        {
            _familyInfoCommand = familyInfoCommand;
            _familyRegisterCommand = familyRegisterCommand;
            _familyInviteListCommand = familyInviteListCommand;
            _familyKickUserCommand = familyKickUserCommand;
            _familyUpdateUserStatusCommand = familyUpdateUserStatusCommand;
            _familyRenameCommand = familyRenameCommand;
            _familyDeleteCommand = familyDeleteCommand;
            _familyUpdateDescriptionCommand = familyUpdateDescriptionCommand;
            _familyCheckInfoCommand = familyCheckInfoCommand;
        }

        [Command("")]
        [Summary("Посмотреть информацию о своей семье")]
        public async Task FamilyInfoTask() =>
            await _familyInfoCommand.Execute(Context);

        [Command("информация"), Alias("info")]
        [Summary("Посмотреть информацию о семье с указанным названием")]
        [CommandUsage("!семья информация Кукушки")]
        public async Task FamilyCheckInfoTask(
            [Summary("Название семьи")] [Remainder] string familyName = null) =>
            await _familyCheckInfoCommand.Execute(Context, familyName);

        [Command("регистрация"), Alias("registry")]
        [Summary("Начать регистрацию семьи с указанным названием")]
        [CommandUsage("!семья регистрация Кукушки")]
        public async Task FamilyRegisterTask(
            [Summary("Название семьи")] [Remainder] string familyName) =>
            await _familyRegisterCommand.Execute(Context, familyName);

        [Command("приглашения"), Alias("invites")]
        [Summary("Посмотреть список приглашений в семью")]
        public async Task FamilyInviteListTask() =>
            await _familyInviteListCommand.Execute(Context);

        [Command("назначить"), Alias("set")]
        [Summary("Назначить новый статус в семье указанному пользователю")]
        [CommandUsage("!семья назначить 1 Холли", "!семья назначить 0 Рыбка")]
        public async Task FamilyUpdateUserStatusTask(
            [Summary("Номер статуса")] UserInFamilyStatus userFamilyStatus,
            [Summary("Игровое имя")] [Remainder] string username) =>
            await _familyUpdateUserStatusCommand.Execute(Context, userFamilyStatus, username);

        [Command("выгнать"), Alias("kick")]
        [Summary("Выгнать указанного пользователя из семьи")]
        [CommandUsage("!семья выгнать Холли")]
        public async Task FamilyKickUserTask(
            [Summary("Игровое имя")] [Remainder] string username) =>
            await _familyKickUserCommand.Execute(Context, username);

        [Command("описание"), Alias("description")]
        [Summary("Сменить описание семьи на новое")]
        [CommandUsage("!семья описание Самые крутые кукушки в деревне")]
        public async Task FamilyUpdateDescriptionTask(
            [Summary("Новое описание")] [Remainder] string newDescription) =>
            await _familyUpdateDescriptionCommand.Execute(Context, newDescription);

        [Command("расформировать"), Alias("delete")]
        [Summary("Расформировать (удалить) семью")]
        public async Task FamilyDeleteTask() =>
            await _familyDeleteCommand.Execute(Context);

        [Command("переименовать"), Alias("rename")]
        [CommandUsage("Сменить название семьи на новое")]
        [Summary("!семья переименовать Золотые кукушки")]
        public async Task FamilyRenameTask(
            [Summary("Новое название семьи")] [Remainder] string newFamilyName) =>
            await _familyRenameCommand.Execute(Context, newFamilyName);

        [Group("приглашение"), Alias("invite")]
        public class FamilyInvitesCommands : ModuleBase<SocketCommandContext>
        {
            private readonly IFamilyInviteSendCommand _familyInviteSendCommand;
            private readonly IFamilyInviteCancelCommand _familyInviteCancelCommand;
            private readonly IFamilyInviteAcceptCommand _familyInviteAcceptCommand;
            private readonly IFamilyInviteDeclineCommand _familyInviteDeclineCommand;

            public FamilyInvitesCommands(IFamilyInviteSendCommand familyInviteSendCommand,
                IFamilyInviteCancelCommand familyInviteCancelCommand,
                IFamilyInviteAcceptCommand familyInviteAcceptCommand,
                IFamilyInviteDeclineCommand familyInviteDeclineCommand)
            {
                _familyInviteSendCommand = familyInviteSendCommand;
                _familyInviteCancelCommand = familyInviteCancelCommand;
                _familyInviteAcceptCommand = familyInviteAcceptCommand;
                _familyInviteDeclineCommand = familyInviteDeclineCommand;
            }

            [Command("отправить"), Alias("send")]
            [Summary("Отправить приглашение в семью указанному пользователя")]
            [CommandUsage("!семья приглашение отправить Холли")]
            public async Task FamilyInviteSendTask(
                [Summary("Игровое имя")] [Remainder] string username) =>
                await _familyInviteSendCommand.Execute(Context, username);

            [Command("отменить"), Alias("cancel")]
            [Summary("Отменить отправленное приглашение в семью")]
            [CommandUsage("!семья приглашение отменить 323")]
            public async Task FamilyInviteCancelTask(
                [Summary("Номер приглашения")] long inviteId) =>
                await _familyInviteCancelCommand.Execute(Context, inviteId);

            [Command("принять"), Alias("accept")]
            [Summary("Принять указанное приглашение в семью")]
            [CommandUsage("!семья приглашение принять 323")]
            public async Task FamilyInviteAcceptTask(
                [Summary("Номер приглашения")] long inviteId) =>
                await _familyInviteAcceptCommand.Execute(Context, inviteId);

            [Command("отказаться"), Alias("decline")]
            [Summary("Отказаться от указанного приглашения в семью")]
            [CommandUsage("!семья приглашения отказаться 323")]
            public async Task FamilyInviteDeclineTask(
                [Summary("Номер приглашения")] long inviteId) =>
                await _familyInviteDeclineCommand.Execute(Context, inviteId);
        }

        [Group("казна"), Alias("currency")]
        public class FamilyCurrencyCommands : ModuleBase<SocketCommandContext>
        {
            private readonly IFamilyCurrencyAddCommand _familyCurrencyAddCommand;
            private readonly IFamilyCurrencyTakeCommand _familyCurrencyTakeCommand;

            public FamilyCurrencyCommands(IFamilyCurrencyAddCommand familyCurrencyAddCommand,
                IFamilyCurrencyTakeCommand familyCurrencyTakeCommand)
            {
                _familyCurrencyAddCommand = familyCurrencyAddCommand;
                _familyCurrencyTakeCommand = familyCurrencyTakeCommand;
            }

            [Command("добавить"), Alias("add")]
            [Summary("Добавить валюту в казну семьи")]
            [CommandUsage("!семья казна добавить 500 иен")]
            public async Task FamilyCurrencyAddTask(
                [Summary("Количество")] long amount,
                [Summary("Название валюты")] [Remainder] string currencyNamePattern) =>
                await _familyCurrencyAddCommand.Execute(Context, amount, currencyNamePattern);

            [Command("взять"), Alias("take")]
            [Summary("Взять валюту из казны семьи")]
            [CommandUsage("!семья казна взять 500 иен")]
            public async Task FamilyCurrencyTakeTask(
                [Summary("Количество")] long amount,
                [Summary("Название валюты")] [Remainder] string currencyNamePattern) =>
                await _familyCurrencyTakeCommand.Execute(Context, amount, currencyNamePattern);
        }
    }
}
