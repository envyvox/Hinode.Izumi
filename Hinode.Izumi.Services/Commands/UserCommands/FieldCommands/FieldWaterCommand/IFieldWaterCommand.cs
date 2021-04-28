﻿using System.Threading.Tasks;
using Discord.Commands;

namespace Hinode.Izumi.Services.Commands.UserCommands.FieldCommands.FieldWaterCommand
{
    public interface IFieldWaterCommand
    {
        Task Execute(SocketCommandContext context, string namePattern = null);
    }
}
