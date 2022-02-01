using GreenPipes;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Management.Core;
using MassTransit.Management.Sagas.Order;
using MassTransit.Management.Sagas.Order.Infrastructure;
using MassTransit.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MassTransit.Management
{
    public static class MassTransitServiceRegistration
    {

        public static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services, List<ConsumerBaseEntity> consumers = null)
        {



            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                //consumers
                if (consumers != null)
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

                    cfg.UseInMemoryOutbox();
                    cfg.ConfigureEndpoints(context);

                });

                #region Saga Configuration
                //saga 
                //x.AddSagaStateMachine<OrderSagaMachine, OrderSagaStatus>()
                //  .InMemoryRepository();

                x.AddSagaStateMachine<OrderSagaMachine, OrderSagaStatus>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // or use Optimistic, which requires RowVersion

                    r.AddDbContext<DbContext, OrderStateDbContext>((provider, builder) =>
                   {
                       builder.UseSqlServer("Server=.; Initial Catalog=GaebSaga-Order; Trusted_Connection = True;", m =>
                       {
                           m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                           m.MigrationsHistoryTable($"__{nameof(OrderStateDbContext)}");
                       });
                   });
                });

                #endregion




            });


            services.AddMassTransitHostedService();




            return services;
        }






    }
}
