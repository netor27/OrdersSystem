using System;
using System.Collections.Generic;
using System.Linq;

using Gbm.Ordering.Domain.Common;
using Gbm.Ordering.Domain.Events;

namespace Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public class Buyer : Entity, IAggregateRoot
    {
        private List<PaymentMethod> paymentMethods;
        public string IdentityGuid { get; private set; }
        public IEnumerable<PaymentMethod> PaymentMethods => paymentMethods.AsReadOnly();

        protected Buyer()
        {
            this.paymentMethods = new List<PaymentMethod>();
        }
        
        public Buyer(string identity) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? 
                identity : throw new ArgumentNullException(nameof(identity));
        }

        public PaymentMethod VerifyOrAddPaymentMethod(CardType cardType, string alias, string cardNumber,
            string securityNumber, string cardHolderName, DateTime expiration, int orderId)
        {
            var paymentMethod = this.paymentMethods.Where(p => p.IsEqual(cardType, cardNumber, expiration))
                                                           .SingleOrDefault();
            if(paymentMethod == null)
            {
                paymentMethod = new PaymentMethod(cardType, alias, cardNumber, securityNumber, cardHolderName, expiration);
                this.paymentMethods.Add(paymentMethod);
            }

            AddDomainEvents(new BuyerAndPaymentMethodVerificationDomainEvent(this, paymentMethod, orderId));
            return paymentMethod;
        }        
    }
}
