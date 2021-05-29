using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldBuyCommand;
using Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldCollectCommand;
using Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldDigCommand;
using Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldInfoCommand;
using Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldPlantCommand;
using Hinode.Izumi.Commands.UserCommands.FieldCommands.FieldWaterCommand;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.WebServices.CommandWebService.Attributes;

namespace Hinode.Izumi.Commands.UserCommands.FieldCommands
{
    [CommandCategory(CommandCategory.Field)]
    [Group("участок"), Alias("field")]
    [IzumiRequireRegistry]
    [IzumiRequireLocation(Location.Village), IzumiRequireNoDebuff(BossDebuff.VillageStop)]
    public class FieldCommands : ModuleBase<SocketCommandContext>
    {
        private readonly IFieldInfoCommand _fieldInfoCommand;
        private readonly IFieldBuyCommand _fieldBuyCommand;
        private readonly IFieldPlantCommand _fieldPlantCommand;
        private readonly IFieldWaterCommand _fieldWaterCommand;
        private readonly IFieldCollectCommand _fieldCollectCommand;
        private readonly IFieldDigCommand _fieldDigCommand;

        public FieldCommands(IFieldInfoCommand fieldInfoCommand, IFieldBuyCommand fieldBuyCommand,
            IFieldPlantCommand fieldPlantCommand, IFieldWaterCommand fieldWaterCommand,
            IFieldCollectCommand fieldCollectCommand, IFieldDigCommand fieldDigCommand)
        {
            _fieldInfoCommand = fieldInfoCommand;
            _fieldBuyCommand = fieldBuyCommand;
            _fieldPlantCommand = fieldPlantCommand;
            _fieldWaterCommand = fieldWaterCommand;
            _fieldCollectCommand = fieldCollectCommand;
            _fieldDigCommand = fieldDigCommand;
        }

        [Command("")]
        [Summary("Посмотреть информацию о своем участке")]
        public async Task FieldInfoTask() =>
            await _fieldInfoCommand.Execute(Context);

        [Command("купить"), Alias("buy")]
        [Summary("Приобрести участок")]
        public async Task FieldBuyTask() =>
            await _fieldBuyCommand.Execute(Context);

        [Command("посадить"), Alias("plant")]
        [Summary("Посадить семена на указанную клетку земли")]
        [CommandUsage("!посадить 1 семя картофеля", "!посадить 3 рассада хмеля")]
        public async Task FieldPlantTask(
            [Summary("Номер клетки")] long fieldId,
            [Summary("Название семян")] [Remainder] string seedNamePattern) =>
            await _fieldPlantCommand.Execute(Context, fieldId, seedNamePattern);

        [Command("полить"), Alias("water")]
        [Summary("Полить свой участок или участок указанного члена семьи")]
        [CommandUsage("!полить", "!полить Холли")]
        public async Task FieldWaterTask(
            [Summary("Игровое имя")] [Remainder] string namePattern = null) =>
            await _fieldWaterCommand.Execute(Context, namePattern);

        [Command("собрать"), Alias("collect")]
        [Summary("Собрать урожай с указанной клетки земли")]
        [CommandUsage("!собрать 1", "!собрать 3")]
        public async Task FieldCollectTask(
            [Summary("Номер клетки")] long fieldId) =>
            await _fieldCollectCommand.Execute(Context, fieldId);

        [Command("выкопать"), Alias("dig")]
        [Summary("Выкопать семена из указанной клетки земли")]
        [CommandUsage("!выкопать 1", "!выкопать 3")]
        public async Task FieldDigTask(
            [Summary("Номер клетки")] long fieldId) =>
            await _fieldDigCommand.Execute(Context, fieldId);
    }
}
