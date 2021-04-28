using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.DiscordEnums;
using Hinode.Izumi.Services.Commands.Attributes;
using Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldBuyCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldCollectCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldDigCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldInfoCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldPlantCommand;
using Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldWaterCommand;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands
{
    [Group("участок"), Alias("field")]
    [IzumiRequireContext(DiscordContext.DirectMessage), IzumiRequireRegistry]
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

        [Command]
        public async Task FieldInfoTask() =>
            await _fieldInfoCommand.Execute(Context);

        [Command("купить"), Alias("buy")]
        public async Task FieldBuyTask() =>
            await _fieldBuyCommand.Execute(Context);

        [Command("посадить"), Alias("plant")]
        public async Task FieldPlantTask(long fieldId, [Remainder] string seedNamePattern) =>
            await _fieldPlantCommand.Execute(Context, fieldId, seedNamePattern);

        [Command("полить"), Alias("water")]
        public async Task FieldWaterTask([Remainder] string namePattern = null) =>
            await _fieldWaterCommand.Execute(Context, namePattern);

        [Command("собрать"), Alias("collect")]
        public async Task FieldCollectTask(long fieldId) =>
            await _fieldCollectCommand.Execute(Context, fieldId);

        [Command("выкопать"), Alias("dig")]
        public async Task FieldDigTask(long fieldId) =>
            await _fieldDigCommand.Execute(Context, fieldId);
    }
}
