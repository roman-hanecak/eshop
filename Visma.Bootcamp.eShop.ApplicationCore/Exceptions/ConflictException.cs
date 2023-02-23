using System;

namespace Visma.Bootcamp.eShop.ApplicationCore.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string message)
            : base(message)
        { }
    }
}
