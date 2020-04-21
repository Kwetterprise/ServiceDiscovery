// ---------------------------------------------------------------------------------------------------------------------
//  <copyright file="PingAvailabilityProvider.cs" company="Prodrive B.V.">
//      Copyright (c) Prodrive B.V. All rights reserved.
//  </copyright>
// ---------------------------------------------------------------------------------------------------------------------

namespace Kwetterprise.ServiceDiscovery.Business.Manager
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Kwetterprise.ServiceDiscovery.Common;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;

    public class PingAvailabilityProvider : IAvailabilityProvider, IDisposable
    {
        private const string pingEndpoint = "/ping";
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<ServiceStatus> GetIsAvailable(Service service)
        {
            try
            {
                using var result = await this.httpClient.GetAsync(service.Url + PingAvailabilityProvider.pingEndpoint);
                return result.StatusCode.ToServiceStatus();
            }
            catch (HttpRequestException)
            {
                return ServiceStatus.Unreachable;
            }
        }

        public void Dispose()
        {
            this.httpClient.Dispose();
        }
    }
}