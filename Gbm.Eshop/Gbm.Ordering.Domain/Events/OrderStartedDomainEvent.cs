using Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate;
using Gbm.Ordering.Domain.AggregatesModel.OrderAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gbm.Ordering.Domain.Events
{
    public class OrderStartedDomainEvent : IAsyncNotification
    {
        public CardType CardType { get; private set; }

        public string CardNumber { get; private set; }

        public string CardSecutiryNumber { get; private set; }

        public string CardHolderName { get; private set; }

        public DateTime CardExpiration { get; private set; }

        public Order Order { get; private set; }

        public OrderStartedDomainEvent(Order order, CardType cardType, string cardNumber, 
            string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
        {
            Order = order;
            CardType = cardType;
            CardNumber = cardNumber;
            CardSecutiryNumber = cardSecurityNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
        }

    }
}
