using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Data.Enums.FamilyEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyCheckInfoCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyInfoCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.BaseCommands.FamilyRegisterCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyAddCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.CurrencyCommands.FamilyCurrencyTakeCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyKickUserCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InteractionCommands.FamilyUpdateUserStatusCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteAcceptCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteCancelCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteDeclineCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteListCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.InviteCommands.FamilyInviteSendCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyDeleteCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyRenameCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands.ManageCommands.FamilyUpdateDescriptionCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.FamilyCommands
{
    [Group("семья"), Alias("family")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
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

        [Command]
        public async Task FamilyInfoTask() =>
            await _familyInfoCommand.Execute(Context);

        [Command("информация"), Alias("info")]
        public async Task FamilyCheckInfoTask([Remainder] string familyName = null) =>
            await _familyCheckInfoCommand.Execute(Context, familyName);

        [Command("регистрация"), Alias("registry")]
        public async Task FamilyRegisterTask([Remainder] string familyName) =>
            await _familyRegisterCommand.Execute(Context, familyName);

        [Command("приглашения"), Alias("invites")]
        public async Task FamilyInviteListTask() =>
            await _familyInviteListCommand.Execute(Context);

        [Command("назначить"), Alias("set")]
        public async Task
            FamilyUpdateUserStatusTask(UserInFamilyStatus userFamilyStatus, [Remainder] string username) =>
            await _familyUpdateUserStatusCommand.Execute(Context, userFamilyStatus, username);

        [Command("выгнать"), Alias("kick")]
        public async Task FamilyKickUserTask([Remainder] string username) =>
            await _familyKickUserCommand.Execute(Context, username);

        [Command("описание"), Alias("description")]
        public async Task FamilyUpdateDescriptionTask([Remainder] string newDescription) =>
            await _familyUpdateDescriptionCommand.Execute(Context, newDescription);

        [Command("расформировать"), Alias("delete")]
        public async Task FamilyDeleteTask() =>
            await _familyDeleteCommand.Execute(Context);

        [Command("переименовать"), Alias("rename")]
        public async Task FamilyRenameTask([Remainder] string newFamilyName) =>
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
            public async Task FamilyInviteSendTask([Remainder] string username) =>
                await _familyInviteSendCommand.Execute(Context, username);

            [Command("отменить"), Alias("cancel")]
            public async Task FamilyInviteCancelTask(long inviteId) =>
                await _familyInviteCancelCommand.Execute(Context, inviteId);

            [Command("принять"), Alias("accept")]
            public async Task FamilyInviteAcceptTask(long inviteId) =>
                await _familyInviteAcceptCommand.Execute(Context, inviteId);

            [Command("отказаться"), Alias("decline")]
            public async Task FamilyInviteDeclineTask(long inviteId) =>
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
            public async Task FamilyCurrencyAddTask(long amount, [Remainder] string currencyNamePattern) =>
                await _familyCurrencyAddCommand.Execute(Context, amount, currencyNamePattern);

            [Command("взять"), Alias("take")]
            public async Task FamilyCurrencyTakeTask(long amount, [Remainder] string currencyNamePattern) =>
                await _familyCurrencyTakeCommand.Execute(Context, amount, currencyNamePattern);
        }
    }
}
