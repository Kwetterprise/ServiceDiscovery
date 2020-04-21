namespace Kwetterprise.ServiceDiscovery.Business.Manager
{
    using System.Net;
    using System.Threading.Tasks;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;

    public interface IAvailabilityProvider
    {
        Task<ServiceStatus> GetIsAvailable(Service service);
    }
}
