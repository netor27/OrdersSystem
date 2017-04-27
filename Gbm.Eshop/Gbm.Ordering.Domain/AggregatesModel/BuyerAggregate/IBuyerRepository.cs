using System.Threading.Tasks;


namespace Gbm.Ordering.Domain.AggregatesModel.BuyerAggregate
{
    public interface IBuyerRepository
    {
        Buyer Add(Buyer buyer);

        Buyer Update(Buyer buyer);
    }
}
