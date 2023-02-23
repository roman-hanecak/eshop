using System;

namespace Visma.Bootcamp.eShop.ApplicationCore.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException(string message)
            : base(message)
        { }
    }
}
