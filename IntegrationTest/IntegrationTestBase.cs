
namespace Kwetterprise.ServiceDiscovery.IntegrationTest
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Kwetterprise.ServiceDiscovery.Web;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class IntegrationTestBase : IDisposable
    {
        protected IHost? host;

        private HttpClient? client;

        protected HttpClient Client => this.client ?? throw new InvalidOperationException("Host is not started yet.");

        public async Task Start(Action<IServiceCollection>? configureServices = null)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer();

                    // Specify the environment
                    webHost.UseEnvironment("Test");

                    webHost.UseStartup<Startup>();
                    
                    if (configureServices != null)
                    {
                        webHost.ConfigureServices(configureServices);
                    }
                });

            this.host = await hostBuilder.StartAsync();

            this.client = this.host.GetTestClient();
        }


        public void Dispose()
        {
            this.host?.Dispose();
        }
    }
}
