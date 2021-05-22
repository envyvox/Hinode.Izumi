using Hinode.Izumi.Services.GameServices.FieldService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FieldService.Queries
{
    public record GetUserFieldsQuery(long UserId) : IRequest<UserFieldRecord[]>;
}
