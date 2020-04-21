namespace Kwetterprise.ServiceDiscovery.Web.Worker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading;
    using System.Threading.Tasks;
    using Kwetterprise.ServiceDiscovery.Business.Manager;
    using Kwetterprise.ServiceDiscovery.Common.Models;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    class ServiceAvailabilityWorker : IHostedService, IDisposable
    {
        private readonly ILogger<ServiceAvailabilityWorker> logger;
        private readonly IServiceManager serviceManager;
        private readonly IAvailabilityProvider availabilityProvider;
        private Timer availabilityCheckTimer = null!;
        private Task runTask = null!;

        public ServiceAvailabilityWorker(ILogger<ServiceAvailabilityWorker> logger, IServiceManager serviceManager, IAvailabilityProvider availabilityProvider)
        {
            this.logger = logger;
            this.serviceManager = serviceManager;
            this.availabilityProvider = availabilityProvider;
        }

        public async Task RunTask()
        {
            this.logger.LogInformation("Running worker task.");

            var all = this.serviceManager.GetAll();

            var serviceStatuses = new Dictionary<Guid, ServiceStatus>();

            foreach (var service in all)
            {
                var statusCode = await this.availabilityProvider.GetIsAvailable(service);
                serviceStatuses.Add(service.Guid, statusCode);
            }

            this.serviceManager.ProcessStatusses(serviceStatuses);
        }

        public void DoWork(object? state)
        {
            this.runTask = this.RunTask();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.availabilityCheckTimer = new Timer(this.DoWork, null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.availabilityCheckTimer.Change(0, Timeout.Infinite);

            var task = this.runTask;
            if (task != null)
            {
                await task;
            }
        }

        public void Dispose()
        {
            this.availabilityCheckTimer?.Dispose();
        }
    }
}
