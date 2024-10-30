using MassTransit;
using MassTransit.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Messaging
{
    public static class MassTransitRegistration
    {
        public static void RegisterMassTransit(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator> registerConsumers, params Assembly[] assemblies)
        {
            services.AddDbContext<MassTransitOutboxContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure()));

            services.AddMassTransit(masstransit =>
            {
                masstransit.AddServiceBusMessageScheduler();

                masstransit.SetKebabCaseEndpointNameFormatter();

                masstransit.AddActivities(assemblies);
                masstransit.AddConsumers(assemblies);

                masstransit.AddEntityFrameworkOutbox<MassTransitOutboxContext>(o =>
                {
                    // configure which database lock provider to use (Postgres, SqlServer, or MySql)
                    o.UseSqlServer();

                    // enable the bus outbox
                    o.UseBusOutbox();
                });

                masstransit.AddConfigureEndpointsCallback((context, name, cfg) =>
                {
                    cfg.UseEntityFrameworkOutbox<MassTransitOutboxContext>(context);
                });

                masstransit.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("AzureServiceBus"));

                    cfg.UseServiceBusMessageScheduler();

                    // Subscribe to OrderSubmitted directly on the topic, instead of configuring a queue
                    registerConsumers(context, cfg);

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
