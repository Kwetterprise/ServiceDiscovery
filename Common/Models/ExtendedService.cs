namespace Kwetterprise.ServiceDiscovery.Common.Models
{
    using System;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;

    /// <summary>
    /// A class representing a service.
    /// </summary>
    public class ExtendedService : Service
    {
        public ExtendedService(Service service, ServiceStatus status)
        :base(service)
        {
            this.Status = status;
        }

        public ServiceStatus Status { get; set; }
    }
}
