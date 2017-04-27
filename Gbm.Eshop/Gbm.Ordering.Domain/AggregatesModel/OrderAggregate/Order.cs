using System;
using Gbm.Ordering.Domain.Common;
using System.Collections.Generic;
using Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate;

namespace Gbm.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private DateTime orderDate;
        private int? buyerId;
        private int orderStatusId;
        private int? paymentMethodId;
        private List<OrderItem> orderItems;

        public Address Address { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public IEnumerable<OrderItem> OrderItems => orderItems.AsReadOnly();

        public Order(Address address, CardType cardType, string cardNumber, string cardSecurityNumber,
            string cardHolderName, DateTime cardExpiration, int? buyerId, int? paymentMethodId)
        {
            this.orderItems = new List<OrderItem>();
            this.buyerId = buyerId;
            this.paymentMethodId = paymentMethodId;
            this.orderStatusId = OrderStatus.InProcess.Id;
            this.orderDate = DateTime.UtcNow;
            this.Address = address;

            // TODO: AddOrderStartedDomainEvent

        }

    }
}
