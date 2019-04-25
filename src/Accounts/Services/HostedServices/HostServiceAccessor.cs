using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace Accounts.Services.HostedServices
{
    public class HostServiceAccessor<T> : IHostedServiceAccessor<T>
    {
        public HostServiceAccessor(IEnumerable<IHostedService> hostedServices)
        {
            foreach (var service in hostedServices)
            {
                if (service is T matchingService)
                {
                    Service = matchingService;
                    break;
                }
            }
        }

        public T Service { get; private set; }
    }
}
