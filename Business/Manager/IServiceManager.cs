namespace Kwetterprise.ServiceDiscovery.Business.Manager
{
    using System;
    using System.Collections.Generic;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;

    public interface IServiceManager
    {
        void Register(Service service);

        void Unregister(Guid guid);

        List<ExtendedService> GetAll();

        void ProcessStatusses(Dictionary<Guid, ServiceStatus> statusses);
    }
}
