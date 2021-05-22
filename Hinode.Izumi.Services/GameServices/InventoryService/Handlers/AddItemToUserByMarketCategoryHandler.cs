﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Hinode.Izumi.Data.Enums;
using Hinode.Izumi.Services.GameServices.InventoryService.Commands;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.InventoryService.Handlers
{
    public class AddItemToUserByMarketCategoryHandler : IRequestHandler<AddItemToUserByMarketCategoryCommand>
    {
        private readonly IMediator _mediator;

        public AddItemToUserByMarketCategoryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Unit> Handle(AddItemToUserByMarketCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var (userId, category, itemId, amount) = request;

            await _mediator.Send(new AddItemToUserByInventoryCategoryCommand(userId, category switch
            {
                MarketCategory.Gathering => InventoryCategory.Gathering,
                MarketCategory.Crafting => InventoryCategory.Crafting,
                MarketCategory.Alcohol => InventoryCategory.Alcohol,
                MarketCategory.Drink => InventoryCategory.Drink,
                MarketCategory.Food => InventoryCategory.Food,
                _ => throw new ArgumentOutOfRangeException()
            }, itemId, amount), cancellationToken);

            return new Unit();
        }
    }
}
