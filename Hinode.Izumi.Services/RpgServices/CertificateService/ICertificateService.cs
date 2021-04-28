using System.Collections.Generic;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Services.RpgServices.CertificateService.Models;

namespace Hinode.Izumi.Services.RpgServices.CertificateService
{
    public interface ICertificateService
    {
        /// <summary>
        /// Возвращает массив сертификатов.
        /// </summary>
        /// <returns>Массив сертификатов.</returns>
        Task<CertificateModel[]> GetAllCertificates();

        /// <summary>
        /// Возвращает сертификат. Кэшируется.
        /// </summary>
        /// <param name="certificateId">Id сертификата.</param>
        /// <returns>Сертификат.</returns>
        /// <exception cref="IzumiNullableMessage.Certificate"></exception>
        Task<CertificateModel> GetCertificate(long certificateId);

        /// <summary>
        /// Возвращает сертификат пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="certificateId">Id сертификата.</param>
        /// <returns>Сертификат.</returns>
        /// <exception cref="IzumiNullableMessage.UserCertificate"></exception>
        Task<CertificateModel> GetUserCertificate(long userId, long certificateId);

        /// <summary>
        /// Возвращает библиотеку сертификатов пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <returns>Библиотека сертификатов.</returns>
        Task<Dictionary<Certificate, CertificateModel>> GetUserCertificate(long userId);

        /// <summary>
        /// Проверяет наличие сертификата у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="certificateId">Id сертификата.</param>
        /// <returns>True если есть, false если нет.</returns>
        Task<bool> CheckUserCertificate(long userId, long certificateId);

        /// <summary>
        /// Добавляет сертификат пользователю.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="certificateId">Id сертификата.</param>
        Task AddCertificateToUser(long userId, long certificateId);

        /// <summary>
        /// Забирает сертификат у пользователя.
        /// </summary>
        /// <param name="userId">Id пользователя.</param>
        /// <param name="certificateId">Id сертификата.</param>
        Task RemoveCertificateFromUser(long userId, long certificateId);
    }
}
