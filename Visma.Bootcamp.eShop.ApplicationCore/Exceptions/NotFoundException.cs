using System;

namespace Visma.Bootcamp.eShop.ApplicationCore.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        { }
    }
}
