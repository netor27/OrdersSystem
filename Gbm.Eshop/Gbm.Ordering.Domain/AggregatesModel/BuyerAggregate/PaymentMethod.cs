using System;
using System.Collections.Generic;

using Gbm.Ordering.Domain.Common;


namespace Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class PaymentMethod : Entity
    {
        private readonly string alias;
        private readonly string cardNumber;
        private readonly string securityNumber;
        private readonly string cardHolderName;
        private readonly DateTime expiration;

        public CardType CardType { get; private set; }

        protected PaymentMethod() { }

        public PaymentMethod(CardType cardType, string alias, string cardNumber, 
            string securityNumber, string cardHolderName, DateTime expiration)
        {

        }
    }
}
