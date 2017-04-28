using System;

using Gbm.Ordering.Domain.Common;
using Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate;

namespace Gbm.Ordering.Infrastructure.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly OrderingContext context;

        public IUnitOfWork UnitOfWork { get { return context; } }

        public BuyerRepository(OrderingContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Buyer Add(Buyer buyer)
        {
            if (buyer.IsTransient())
            {
                return context.Buyers.Add(buyer).Entity;
            }
            else
            {
                return buyer;
            }
        }

        public Buyer Update(Buyer buyer)
        {
            return context.Buyers.Update(buyer).Entity;
        }
    }
}
