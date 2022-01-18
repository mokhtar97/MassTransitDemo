using GreenPipes;
using MassTransit.Management.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassTransit.Management
{
    public static class MassTransitServiceRegistration
    {
        public static IServiceCollection AddMassTransitPublisher(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq();
            });

            services.AddMassTransitHostedService();


            return services;
        }

        public static IServiceCollection AddMassTransitConsumer(this IServiceCollection services,  List<ConsumerBaseEntity> consumers) 
        {
            
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();


                #region RetryLimit
                //x.AddConsumer<T>(
                //    c =>
                //    {
                //        c.UseMessageRetry(r =>
                //        {
                //            r.Interval(2, 10000);
                //            // r.Handle<DataException>(x => x.Message.Contains("SQL"));
                //        });
                //        c.UseInMemoryOutbox();
                //    }
                //    )
                //.Endpoint(c => c.InstanceId = instanceId); 
                #endregion
                int instanceCount = 0;
                  foreach (var consumer in consumers)
                   {
                   x.AddConsumer(consumer.ConsumerType).Endpoint(c => c.InstanceId = consumer.InstanceId);
                    instanceCount++;
                  }

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                            {
                                r.Interval(2, 10000);
                                // r.Handle<DataException>(x => x.Message.Contains("SQL"));
                            });
                    cfg.ConfigureEndpoints(context);
                });

            });
            services.AddMassTransitHostedService();



            return services;
        }



      
    }
}
