using Gbm.Ordering.Domain.Common;

namespace Gbm.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order Add(Order order);

        Order Update(Order order);
    }
}
