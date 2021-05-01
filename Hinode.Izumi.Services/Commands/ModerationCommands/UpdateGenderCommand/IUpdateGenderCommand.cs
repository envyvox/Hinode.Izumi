﻿using System.Threading.Tasks;
using Discord.Commands;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.Commands.ModerationCommands.UpdateGenderCommand
{
    public interface IUpdateGenderCommand
    {
        Task Execute(SocketCommandContext context, long userId, Gender gender);
    }
}
