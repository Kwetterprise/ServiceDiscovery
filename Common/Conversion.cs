using System;
using System.Collections.Generic;
using System.Text;

namespace Kwetterprise.ServiceDiscovery.Common
{
    using System.Net;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;

    public static class Conversion
    {
        public static ServiceStatus ToServiceStatus(this HttpStatusCode code)
            => code switch
            {
                HttpStatusCode.OK => ServiceStatus.Ok,
                HttpStatusCode.ServiceUnavailable => ServiceStatus.Unavailable,
                _ => ServiceStatus.Unreachable,
            };
    }
}
