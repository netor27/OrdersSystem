using Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate;
using MediatR;

namespace Gbm.Ordering.Domain.Events
{
    public class BuyerAndPaymentMethodVerificationDomainEvent : IAsyncNotification
    {
        public Buyer Buyer { get; private set; }

        public PaymentMethod Payment { get; private set; }

        public int OrderId { get; private set; }

        public BuyerAndPaymentMethodVerificationDomainEvent(Buyer buyer, PaymentMethod payment, int orderId)
        {
            Buyer = buyer;
            Payment = payment;
            OrderId = orderId;
        }

    }
}
