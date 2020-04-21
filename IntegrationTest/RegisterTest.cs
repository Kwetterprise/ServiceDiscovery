namespace Kwetterprise.ServiceDiscovery.IntegrationTest
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Kwetterprise.ServiceDiscovery.Business.Manager;
    using Kwetterprise.ServiceDiscovery.Common.DataTransfer;
    using Kwetterprise.ServiceDiscovery.Common.Models;
    using Kwetterprise.ServiceDiscovery.IntegrationTest.Mock;
    using Kwetterprise.ServiceDiscovery.Web.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class RegisterTest : IntegrationTestBase
    {
        private readonly MockAvailabilityProvider mockAvailabilityProvider;
        private readonly ServiceManager serviceManager;

        public RegisterTest()
        {
            this.mockAvailabilityProvider = new MockAvailabilityProvider();

            this.Start(
                services =>
                {
                    services.Replace<IAvailabilityProvider, MockAvailabilityProvider>(
                        _ => this.mockAvailabilityProvider,
                        ServiceLifetime.Singleton);
                }).GetAwaiter().GetResult();
            this.serviceManager = (ServiceManager)this.host!.Services.GetService<IServiceManager>();
        }

        [Fact]
        public async Task GetReturnsMethodNotAllowed()
        {
            var response = await this.Client.GetAsync("/register");

            response.StatusCode.AssertEqual(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task Register()
        {
            var service = new Service(Guid.NewGuid(), "service-name", "some-url"); // TODO: Malformed url?

            var registerResponse = await this.Client.PostAsync("/register", service.ToContent());

            var content = await registerResponse.Content.ReadAsStringAsync();

            registerResponse.StatusCode.AssertEqual(HttpStatusCode.OK);

            this.serviceManager.GetAll().AssertSingle().AssertEqual(service);
        }

        [Fact]
        public async Task Unregister()
        {
            var service = new Service(Guid.NewGuid(), "service-name", "some-url"); // TODO: Malformed url?

            var registerResponse = await this.Client.PostAsync("/register", service.ToContent());

            registerResponse.StatusCode.AssertEqual(HttpStatusCode.OK);

            var json2 = new Dictionary<string, string>
            {
                {
                    ExtendedService.GuidPropertyName,
                    service.Guid.ToString()
                }
            }.ToJson();

            var unregisterResponse = await this.Client.DeleteAsync($"/unregister?guid={service.Guid}");

            unregisterResponse.StatusCode.AssertEqual(HttpStatusCode.OK);

            this.serviceManager.GetAll().AssertEmpty();
        }
    }
}
