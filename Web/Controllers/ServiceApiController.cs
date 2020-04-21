using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kwetterprise.ServiceDiscovery.Web.Controllers
{
    using Kwetterprise.ServiceDiscovery.Business.Manager;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;
    using Kwetterprise.ServiceDiscovery.Web.Models;

    [ApiController]
    [Route("")]
    public class ServiceApiController : ControllerBase
    {
        private readonly ILogger<ServiceApiController> _logger;
        private readonly IServiceManager serviceManager;

        public ServiceApiController(ILogger<ServiceApiController> logger, IServiceManager serviceManager)
        {
            this._logger = logger;
            this.serviceManager = serviceManager;
        }

        [HttpGet("GetAll")]
        public List<ExtendedService> GetAll()
        {
            return this.serviceManager.GetAll();
        }

        [HttpPost("Register")]
        public void Register([FromBody] Service service)
        {
            this._logger.LogInformation($"Registering service: {service}.");
            this.serviceManager.Register(service);
        }

        [HttpDelete("Unregister")]
        public void Unregister([FromQuery] Guid guid)
        {
            this._logger.LogInformation($"Unregistering service: {guid}.");
            this.serviceManager.Unregister(guid);
        }
    }
}
