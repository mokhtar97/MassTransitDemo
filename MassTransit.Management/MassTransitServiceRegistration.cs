using GreenPipes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassTransit.Management
{
    public static class MassTransitServiceRegistration
    {
        public static IServiceCollection AddMassTransitServices(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });

            services.AddMassTransitHostedService();


            return services;
        }

        public static IServiceCollection AddMassTransitConsumerServices<T>(this IServiceCollection services, T consumer, string instanceId) where T : class
        {
            
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

             
                x.AddConsumer<T>(
                    c =>
                    {
                        c.UseMessageRetry(r =>
                        {
                            r.Interval(2, 10000);
                            // r.Handle<DataException>(x => x.Message.Contains("SQL"));
                        });
                        c.UseInMemoryOutbox();
                    }
                    )
                .Endpoint(c => c.InstanceId = instanceId);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });

            });
            services.AddMassTransitHostedService();



            return services;
        }



      
    }
}
