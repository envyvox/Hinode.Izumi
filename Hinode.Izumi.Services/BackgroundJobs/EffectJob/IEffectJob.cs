using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums.EffectEnums;

namespace Hinode.Izumi.Services.BackgroundJobs.EffectJob
{
    public interface IEffectJob
    {
        /// <summary>
        /// Снимает эффект с пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="effect">Эффект.</param>
        Task RemoveEffectFromUser(long userId, Effect effect);
    }
}
