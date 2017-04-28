using System;

using Gbm.Ordering.Domain.AggregatesModel.OrderAggregate;
using Gbm.Ordering.Domain.Common;

namespace Gbm.Ordering.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderingContext context;

        public IUnitOfWork UnitOfWork { get { return context; } }

        public OrderRepository(OrderingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Order Add(Order order)
        {
            if (order.IsTransient())
            {
                return context.Orders.Add(order).Entity;
            }
            else
            {
                return order;
            }
        }

        public Order Update(Order order)
        {
            return context.Orders.Update(order).Entity;
        }
    }
}
