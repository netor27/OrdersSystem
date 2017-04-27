using System;

namespace Gbm.Ordering.Infrastructure
{
    public class ClientRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Time { get; set; }
    }
}
