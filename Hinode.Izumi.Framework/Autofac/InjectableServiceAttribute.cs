using System;

namespace Hinode.Izumi.Framework.Autofac
{
    public class InjectableServiceAttribute : Attribute
    {
        public bool IsSingletone { get; set; }
    }
}
