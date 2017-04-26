using System;

namespace Gbm.Ordering.Domain.Exceptions
{
    public class OrderDomainException : Exception
    {
        public OrderDomainException() { }

        public OrderDomainException(string message) : base(message) { }

        public OrderDomainException(string message, Exception inner): base(message, inner) { }
    }
}
