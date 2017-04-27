using Gbm.Ordering.Domain.Common;
using Gbm.Ordering.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gbm.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderItem : Entity
    {
        private string productName;
        private Uri pictureUri;
        private decimal unitPrice;
        private decimal discount;
        private int units;

        public int ProductId { get; set; }

        public OrderItem(int productId, string productName, 
            decimal unitPrice, decimal discount, int units, Uri pictureUri)
        {
            if(units <= 0)
            {
                throw new OrderDomainException("Invalid number of units");
            }

            // TODO: Apply Extract Class Refactoring: to move this rules
            if((unitPrice * units) < discount)
            {
                throw new OrderDomainException("The total of the order item is lower than the applied discount");
            }

            this.ProductId = productId;
            this.productName = productName;
            this.unitPrice = unitPrice;
            this.discount = discount;
            this.units = units;
            this.pictureUri = pictureUri;
        }

        public void SetPictureUri(Uri pictureUri)
        {
            if (pictureUri != null)
            {
                this.pictureUri = pictureUri;
            }
        }

        public void SetNewDiscount(decimal discount)
        {
            if(discount <= 0)
            {
                throw new OrderDomainException("Discount amount is invalid");
            }

            this.discount = discount;
        }

        public decimal GetCurrentDiscount()
        {
            return this.discount;
        }

        public void SetUnits(int units)
        {
            if (units <= 0)
            {
                throw new OrderDomainException("Invalid units");
            }

            this.units = units;
        }
    }
}
