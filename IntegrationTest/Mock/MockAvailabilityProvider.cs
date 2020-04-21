using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetterprise.ServiceDiscovery.IntegrationTest.Mock
{
    using System.Net;
    using System.Threading.Tasks;
    using Kwetterprise.ServiceDiscovery.Business.Manager;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;

    class MockAvailabilityProvider : IAvailabilityProvider
    {
        public Task<ServiceStatus> GetIsAvailable(Service _)
        {
            return Task.FromResult(ServiceStatus.Ok);
        }
    }
}
