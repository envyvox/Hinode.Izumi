using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.PropertyEnums;
using Hinode.Izumi.Services.RpgServices.PropertyService.Models;

namespace Hinode.Izumi.Services.RpgServices.PropertyService
{
    public interface IPropertyService
    {
        /// <summary>
        /// Возвращает значение свойства из базы. Кэшируется.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <returns>Значение свойства.</returns>
        Task<long> GetPropertyValue(Property property);

        /// <summary>
        /// Возвращает свойства мастерства из базы. Кэшируется.
        /// </summary>
        /// <param name="property">Свойство мастерства.</param>
        /// <returns>Свойства мастерства.</returns>
        Task<MasteryPropertyModel> GetMasteryProperty(MasteryProperty property);

        /// <summary>
        /// Возвращает свойства опыта мастерства из базы. Кэшируется.
        /// </summary>
        /// <param name="property">Свойство опыта мастерства.</param>
        /// <returns>Свойства опыта мастерства.</returns>
        Task<MasteryXpPropertyModel> GetMasteryXpProperty(MasteryXpProperty property);

        /// <summary>
        /// Возвращает свойства собирательского ресурса. Кэшируется.
        /// </summary>
        /// <param name="gatheringId">Id собирательского ресурса.</param>
        /// <param name="property">Свойство собирательского ресурса.</param>
        /// <returns>Свойства собирательского ресурса.</returns>
        Task<GatheringPropertyModel> GetGatheringProperty(long gatheringId, GatheringProperty property);

        /// <summary>
        /// Возвращает свойства изготавливаемого предмета. Кэшируется.
        /// </summary>
        /// <param name="craftingId">Id изготавливаемого предмета.</param>
        /// <param name="property">Свойство изготавливаемого предмета.</param>
        /// <returns>Свойства изготавливаемого предмета.</returns>
        Task<CraftingPropertyModel> GetCraftingProperty(long craftingId, CraftingProperty property);

        /// <summary>
        /// Возвращает свойства алкоголя. Кэшируется.
        /// </summary>
        /// <param name="alcoholId">Id алкоголя.</param>
        /// <param name="property">Свойство алкоголя.</param>
        /// <returns>Свойства алкоголя.</returns>
        Task<AlcoholPropertyModel> GetAlcoholProperty(long alcoholId, AlcoholProperty property);

        /// <summary>
        /// Обновляет значение свойства на новое.
        /// </summary>
        /// <param name="property">Свойство.</param>
        /// <param name="newValue">Новое значение свойства.</param>
        Task UpdateProperty(Property property, long newValue);
    }
}
