using GreenPipes;
using MassTransit.Management.Core;
using MassTransit.Management.Sagas.Order;
using MassTransit.Saga;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MassTransit.Management
{
    public static class MassTransitServiceRegistration
    {

        //public static IServiceCollection AddMassTransitPublisher(this IServiceCollection services)
        //{
        //    services.AddMassTransit(x =>
        //    {
        //        x.UsingRabbitMq();
        //    });

        //    services.AddMassTransitHostedService();


        //    return services;
        //}

        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services, List<ConsumerBaseEntity> consumers = null) 
        {



            services.AddMassTransit(x =>
            {              
                x.SetKebabCaseEndpointNameFormatter();

                //consumers
                if(consumers != null)
                {
                    int instanceCount = 0;
                    foreach (var consumer in consumers)
                    {
                        x.AddConsumer(consumer.ConsumerType).Endpoint(c => c.InstanceId = consumer.InstanceId);
                        instanceCount++;
                    }
                }
                

                //middleware like retry limit ,handle error ....
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(r =>
                            {
                                r.Interval(2, 10000);
                            });
                    cfg.ConfigureEndpoints(context);
                 
                });

                //saga 
                x.AddSagaStateMachine<OrderSagaMachine, OrderSagaStatus>()
                  .InMemoryRepository();

            });
            services.AddMassTransitHostedService();



            return services;
        }






    }
}
