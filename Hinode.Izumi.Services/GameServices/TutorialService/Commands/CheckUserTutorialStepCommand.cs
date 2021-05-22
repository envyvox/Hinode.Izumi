﻿using Hinode.Izumi.Data.Enums;
using MediatR;

namespace Hinode.Izumi.Services.GameServices.TutorialService.Commands
{
    public record CheckUserTutorialStepCommand(long UserId, TutorialStep Step) : IRequest;
}
