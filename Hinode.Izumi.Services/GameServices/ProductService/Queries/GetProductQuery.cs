using Hinode.Izumi.Services.GameServices.ProductService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.ProductService.Queries
{
    public record GetProductQuery(long Id) : IRequest<ProductRecord>;
}
