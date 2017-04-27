using System;
using System.Collections.Generic;

using Gbm.Ordering.Domain.Common;
using Gbm.Ordering.Domain.Exceptions;

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
            this.cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : 
                throw new OrderDomainException(nameof(cardNumber));
            this.securityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber :
                throw new OrderDomainException(nameof(securityNumber));
            this.cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName :
                throw new OrderDomainException(nameof(cardHolderName));

            if(expiration <= DateTime.Now)
            {
                throw new OrderDomainException(nameof(expiration));
            }

            this.alias = alias;
            this.expiration = expiration;
            this.CardType = cardType;
        }

        public bool IsEqual(CardType cardType, string cardNumber, DateTime expiration)
        {
            return this.CardType.Id == cardType.Id && this.cardNumber == cardNumber 
                && this.expiration == expiration;
        }
    }
}
