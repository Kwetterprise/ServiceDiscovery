using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kwetterprise.ServiceDiscovery.Web.Models
{
    using Kwetterprise.ServiceDiscovery.Common.Models;

    public class ServiceInfo
    {
        public ILookup<string, ExtendedService> Services;

        public ServiceInfo(ILookup<string, ExtendedService> services)
        {
            this.Services = services;
        }
    }
}
