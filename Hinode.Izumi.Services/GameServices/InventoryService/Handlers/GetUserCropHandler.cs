using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Hinode.Izumi.Data.Enums.MessageEnums;
using Hinode.Izumi.Framework.Database;
using Hinode.Izumi.Services.EmoteService.Queries;
using Hinode.Izumi.Services.Extensions;
using Hinode.Izumi.Services.GameServices.CropService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Queries;
using Hinode.Izumi.Services.GameServices.InventoryService.Records;
using Hinode.Izumi.Services.GameServices.LocalizationService;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class GetUserCropHandler : IRequestHandler<GetUserCropQuery, UserCropRecord>
    {
        private readonly IConnectionManager _con;
        private readonly IMediator _mediator;
        private readonly ILocalizationService _local;

        public GetUserCropHandler(IConnectionManager con, IMediator mediator, ILocalizationService local)
        {
            _con = con;
            _mediator = mediator;
            _local = local;
        }

        public async Task<UserCropRecord> Handle(GetUserCropQuery request, CancellationToken cancellationToken)
        {
            var (userId, cropId) = request;

            var res = await _con.GetConnection()
                .QueryFirstOrDefaultAsync<UserCropRecord>(@"
                    select uc.*, c.name, s.season from user_crops as uc
                        inner join crops c
                            on c.id = uc.crop_id
                        inner join seeds s
                            on s.id = c.seed_id
                    where uc.user_id = @userId
                      and uc.crop_id = @cropId",
                    new {userId, cropId});

            if (res is not null) return res;

            var emotes = await _mediator.Send(new GetEmotesQuery(), cancellationToken);
            var crop = await _mediator.Send(new GetCropByIdQuery(cropId), cancellationToken);

            await Task.FromException(new Exception(IzumiNullableMessage.UserInventory.Parse(
                emotes.GetEmoteOrBlank(crop.Name), _local.Localize(crop.Name, 2))));

            return null;
        }
    }
}
