using System;
using System.Threading.Tasks;

namespace Gbm.Ordering.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistsAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
