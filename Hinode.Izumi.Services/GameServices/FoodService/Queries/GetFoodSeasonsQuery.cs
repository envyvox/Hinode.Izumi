using System.Collections.Generic;
using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.FoodService.Queries
{
    public record GetFoodSeasonsQuery(long Id) : IRequest<List<Season>>;
}
