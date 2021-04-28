using System.Threading.Tasks;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Services.DiscordServices.DiscordGuildService;

namespace Hinode.Izumi.Services.BackgroundJobs.MessageJob
{
    [InjectableService]
    public class MessageJob : IMessageJob
    {
        private readonly IDiscordGuildService _discordGuildService;

        public MessageJob(IDiscordGuildService discordGuildService)
        {
            _discordGuildService = discordGuildService;
        }

        public async Task Delete(long channelId, long messageId)
        {
            // получаем сообщение
            var message = await _discordGuildService.GetIUserMessage(channelId, messageId);
            // удаляем его
            await message.DeleteAsync();
        }
    }
}
