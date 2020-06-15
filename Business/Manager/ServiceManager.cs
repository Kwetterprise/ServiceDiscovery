namespace Kwetterprise.ServiceDiscovery.Business.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;
    using Microsoft.Extensions.Logging;

    public class ServiceManager : IServiceManager
    {
        private readonly ILogger<ServiceManager> logger;
        private readonly Dictionary<Guid, ExtendedService> services = new Dictionary<Guid, ExtendedService>();

        public ServiceManager(ILogger<ServiceManager> logger)
        {
            this.logger = logger;
        }

        public void Register(Service service)
        {
            if (this.services.ContainsKey(service.Guid))
            {
                return;
            }

            var duplicate = this.services.SingleOrDefault(x => x.Value.Url == service.Url && x.Key != service.Guid);

            this.services.Remove(duplicate.Key);
            this.services.Add(service.Guid, new ExtendedService(service, ServiceStatus.Ok));
        }

        public void Unregister(Guid guid)
        {
            if (!this.services.ContainsKey(guid))
            {
                // It's ok to swallow this error.
                return;
            }

            this.services.Remove(guid);
        }

        public List<ExtendedService> GetAll()
        {
            return this.services.Values.ToList();
        }

        public void ProcessStatusses(Dictionary<Guid, ServiceStatus> statuses)
        {
            foreach (var (guid, status) in statuses)
            {
                this.services[guid].Status = status;

                if (status == ServiceStatus.Unreachable)
                {
                    this.logger.LogInformation($"Removing unreachable service {guid}.");
                    this.services.Remove(guid);
                }
            }
        }
    }
}