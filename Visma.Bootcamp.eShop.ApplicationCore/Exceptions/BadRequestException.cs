using System;

namespace Visma.Bootcamp.eShop.ApplicationCore.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        { }
    }
}
