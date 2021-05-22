﻿using Hinode.Izumi.Services.GameServices.CardService.Records;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.CardService.Queries
{
    public record GetUserDeckQuery(long UserId) : IRequest<CardRecord[]>;
}
