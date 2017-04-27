using System;
using Gbm.Ordering.Domain.Common;
using System.Collections.Generic;
using Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate;
using Gbm.Ordering.Domain.Events;
using System.Linq;

namespace Gbm.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        private DateTime orderDate;
        private int? buyerId;
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
            this.OrderStatus = OrderStatus.InProcess;
            this.orderDate = DateTime.UtcNow;
            this.Address = address;

            // TODO: AddOrderStartedDomainEvent
            AddOrderStartedDomainEvent(cardType, cardNumber,
                cardSecurityNumber, cardHolderName, cardExpiration);
        }

        private void AddOrderStartedDomainEvent(CardType cardType, string cardNumber,
            string cardSecurityNumber, string cardHolderName, DateTime cardExpiration)
        {
            var orderStartedDomainEvent = new OrderStartedDomainEvent(this, cardType, cardNumber,
                cardSecurityNumber, cardHolderName, cardExpiration);
            AddDomainEvents(orderStartedDomainEvent);
        }

        public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, 
            Uri pictureUri, int units)
        {
            var existingOrderForProduct = orderItems.Where(o => o.ProductId == productId).SingleOrDefault();
            if(existingOrderForProduct != null)
            {
                existingOrderForProduct.AddUnits(units);
                if (discount > existingOrderForProduct.GetCurrentDiscount())
                {
                    existingOrderForProduct.SetNewDiscount(discount);
                }
            }
            else
            {
                orderItems.Add(new OrderItem(productId, productName, unitPrice, discount, units, pictureUri));
            }
        }

        public void SetPayment(int id)
        {
            paymentMethodId = id;
        }

        public void SetBuyerId(int id)
        {
            buyerId = id;
        }
    }
}
