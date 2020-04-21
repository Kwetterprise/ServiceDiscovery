using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kwetterprise.ServiceDiscovery.Web.Worker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kwetterprise.ServiceDiscovery.Web
{
    using Kwetterprise.ServiceDiscovery.Business.Manager;
    using Microsoft.AspNetCore.Server.Kestrel.Core;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure url.
            services.Configure<KestrelServerOptions>(this.Configuration.GetSection("Kestrel"));

            services.AddSingleton<IServiceManager, ServiceManager>();
            services.AddSingleton<IAvailabilityProvider, PingAvailabilityProvider>();

            services.AddControllers();
            services.AddMvc();

            services.AddHostedService<ServiceAvailabilityWorker>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
