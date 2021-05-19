using System;
using Hinode.Izumi.Data.Enums;

namespace Hinode.Izumi.Services.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CommandCategoryAttribute : Attribute
    {
        public CommandCategory[] Categories { get; }

        public CommandCategoryAttribute(params CommandCategory[] categories) => Categories = categories;
    }
}
