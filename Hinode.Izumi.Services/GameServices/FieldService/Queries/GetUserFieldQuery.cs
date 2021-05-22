using Hinode.Izumi.Services.GameServices.FieldService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Queries
{
    public record GetUserFieldQuery(long UserId, long FieldId) : IRequest<UserFieldRecord>;
}
