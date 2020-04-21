using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kwetterprise.ServiceDiscovery.Web.Controllers
{
    using Kwetterprise.ServiceDiscovery.Business.Manager;

    [Route("")]
    public class WebController : Controller
    {
        private readonly IServiceManager serviceManager;

        public WebController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }

        public IActionResult Index()
        {
            return this.View(new Models.ServiceInfo(this.serviceManager.GetAll().ToLookup(x => x.Name)));
        }
    }
}