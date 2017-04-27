using Gbm.Ordering.Domain.Common;

namespace Gbm.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class OrderStatus : Enumeration<OrderStatus>
    {
        public static OrderStatus InProcess = new OrderStatus
            (1, nameof(InProcess).ToLowerInvariant());
        public static OrderStatus Shipped = new OrderStatus
            (2, nameof(Shipped).ToLowerInvariant());
        public static OrderStatus Cancelled = new OrderStatus
            (3, nameof(Cancelled).ToLowerInvariant());

        protected OrderStatus(int id, string name) : base(id, name) { }
    }
}
