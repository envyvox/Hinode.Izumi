using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Autofac;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.RpgServices.CertificateService.Models;
using Microsoft.Extensions.Caching.Memory;
using CacheExtensions = Hinode.Izumi.Services.Extensions.CacheExtensions;

namespace Hinode.Izumi.Services.RpgServices.CertificateService.Impl
{
    [InjectableService]
    public class CertificateService : ICertificateService
    {
        private readonly IConnectionManager _con;
        private readonly IMemoryCache _cache;

        public CertificateService(IConnectionManager con, IMemoryCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public async Task<CertificateModel[]> GetAllCertificates() =>
            (await _con.GetConnection()
                .QueryAsync<CertificateModel>(@"
                    select * from certificates
                    order by type"))
            .ToArray();

        public async Task<CertificateModel> GetCertificate(long certificateId)
        {
            // проверяем сертификат в кэше
            if (_cache.TryGetValue(string.Format(CacheExtensions.CertificateKey, certificateId), out CertificateModel certificate))
                return certificate;

            // получаем сертификат из базы
            certificate = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CertificateModel>(@"
                    select * from certificates
                    where id = @certificateId",
                    new {certificateId});

            // если такой сертификат есть
            if (certificate != null)
            {
                // добавляем его в кэш
                _cache.Set(string.Format(CacheExtensions.CertificateKey, certificateId), certificate,
                    CacheExtensions.DefaultCacheOptions);
                // и возвращаем
                return certificate;
            }

            // если такого сертификата нет - выводим ошибку
            await Task.FromException(new Exception(IzumiNullableMessage.Certificate.Parse()));
            return new CertificateModel();
        }

        public async Task<CertificateModel> GetUserCertificate(long userId, long certificateId)
        {
            // получаем сертификат пользователя
            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<CertificateModel>(@"
                    select c.* from user_certificates as uc
                        inner join certificates c
                            on c.id = uc.certificate_id
                    where uc.user_id = @userId
                      and uc.certificate_id = @certificateId",
                    new {userId, certificateId});

            // если у пользователя нет сертификата - выводим ошибку
            if (res == null)
                await Task.FromException(new Exception(IzumiNullableMessage.UserCertificate.Parse()));

            // возвращаем сертификат
            return res ?? new CertificateModel();
        }

        public async Task<Dictionary<Certificate, CertificateModel>> GetUserCertificate(long userId) =>
            (await _con.GetConnection()
                .QueryAsync<CertificateModel>(@"
                    select c.* from user_certificates as uc
                        inner join certificates c on
                            c.id = uc.certificate_id
                    where uc.user_id = @userId
                    order by type",
                    new {userId}))
            .ToDictionary(x => x.Type);

        public async Task<bool> CheckUserCertificate(long userId, long certificateId) =>
            await _con.GetConnection()
                .QueryFirstOrDefaultAsync<bool>(@"
                    select 1 from user_certificates
                    where user_id = @userId
                      and certificate_id = @certificateId",
                    new {userId, certificateId});

        public async Task AddCertificateToUser(long userId, long certificateId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    insert into user_certificates(user_id, certificate_id)
                    values (@userId, @certificateId)
                    on conflict (user_id, certificate_id) do nothing",
                    new {userId, certificateId});

        public async Task RemoveCertificateFromUser(long userId, long certificateId) =>
            await _con.GetConnection()
                .ExecuteAsync(@"
                    delete from user_certificates
                    where user_id = @userId
                      and certificate_id = @certificateId",
                    new {userId, certificateId});
    }
}
