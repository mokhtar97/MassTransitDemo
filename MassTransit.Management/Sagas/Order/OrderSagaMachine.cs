using Automatonymous;
using MassTransit.Messages.Models.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Management.Sagas.Order
{
    public class OrderSagaMachine : MassTransitStateMachine<OrderSagaStatus>
    {
        public OrderSagaMachine()
        {
            InstanceState(s => s.CurrentState);

            Event(() => OrderCreate, x =>
            x.CorrelateBy(order => order.OrderId.ToString(),
            context => context.Message.ToString())
            .SelectId(context => Guid.NewGuid()));


            Initially(
               When(OrderCreate)
               .Then(context =>
               {
                   // Will be done by auto mapper                                      
                   context.Instance.OrderId = context.Data.OrderId;
                   context.Instance.Name = context.Data.Name;
                   context.Instance.Amount = context.Data.Amount;
               }).
               If(context => context.Data.Amount == 0, x =>
                   x.Publish(context => new OrderFailedCreatedEvent() { Id= context.Data.OrderId })));
                 //  .TransitionTo(ProcessPayment);



        }

        public Event<OrderCreatedEvent> OrderCreate { get; private set; }
    }
}
